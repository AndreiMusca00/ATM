// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;

public class Program
{
	BankAccount selectedBankAccount = null;
	List<Transaction> transactions = new List<Transaction>();
	
	class Transaction
    {
		
		public string TransactionType { get; set; }
		public int TransactionValue { get; set; }
		public string BankAccount { get; set; }
		public Transaction(string type, int value,string id)
        {
			TransactionType = type;
			TransactionValue = value;
			BankAccount = id;
        }
    }
	class Bill
    {
		public string Name { get; set; }
		public int Cost { get; set; }
		public Bill(string Name,int Cost)
        {
			this.Name = Name;
			this.Cost = Cost;
        }
    }
	class BankAccount
	{
		
		public string OwnerName { get; set; }
		public string Id { get; set; }
		public int Balance { get; set; }
		public bool IsLocked { get; set; }

		List<Bill> billList = new List<Bill>();
		public BankAccount(string OwnerName, string Id, int Balance)
		{
			this.OwnerName = OwnerName;
			this.Id = Id;
			this.Balance = Balance;
			this.IsLocked = false;
			Random rnd = new Random();
			billList.Add(new Bill("1.Electricity",rnd.Next(100,200)));
			billList.Add(new Bill("2.Gas", rnd.Next(100, 200)));
			billList.Add(new Bill("3.Water", rnd.Next(100, 200)));
		}

		public List<Bill> GetBills()
        {
			return billList;
        }
		public void DeleteBill(Bill bill)
        {
			billList.Remove(bill);
        }
		public Bill GetBill(string name)
        {
			foreach(var bill in billList)
            {
				if (bill.Name == name)
					return bill;
            }
			return null;
        }
	}
	List<BankAccount> accounts;
	//2851
	//2995
	Program()
	{
		accounts = new List<BankAccount>
		{
			new BankAccount("Moga Rares","2851",-15),
			new BankAccount("Alex Paul","2995",592)
		};
	}

	void ShowUserOptions()
	{
		Console.WriteLine("1.Insert Card");
		Console.WriteLine("2.Block Card");
	}
	void ShowCardOptions()
    {
		Console.WriteLine("1.Withdraw money");
		Console.WriteLine("2.Deposit");
		Console.WriteLine("3.Pay bills");
		Console.WriteLine("4.Show balance ");
		Console.WriteLine("5.Withdraw card");
		Console.WriteLine("6.Show transactions history");
	}

	void WithdrawMoney()
	{
		Console.WriteLine("Select the amount you want to withdraw");
		int wamount;
		if (int.TryParse(Console.ReadLine(), out wamount))
		{ 
			if(selectedBankAccount.Balance<=wamount)
            {
				Console.WriteLine("Not enough funds");
				return;
            }
			selectedBankAccount.Balance -= wamount;
			Console.WriteLine("Operation great success");
			//adaugare tranzactie 
			Transaction tr = new Transaction("withdraw",wamount,selectedBankAccount.Id);
			transactions.Add(tr);
			
		}
	}
	void DepositMoney()
    {
		Console.WriteLine("How much money do you want to deposit?");
		int amount;
		if (int.TryParse(Console.ReadLine(), out amount))
        {
			selectedBankAccount.Balance += amount;
			Console.WriteLine("Money deposited");
			//adaugare tranzactie 
			Transaction tr = new Transaction("deposit", amount, selectedBankAccount.Id);
			transactions.Add(tr);
		}
    }
	void ShowBalance()
    {
		Console.WriteLine("Current balance:" + selectedBankAccount.Balance);
		//adaugare tranzactie 
		Transaction tr = new Transaction("Balance Check", selectedBankAccount.Balance, selectedBankAccount.Id);
		transactions.Add(tr);
	}
	void PayBills()
    {
		List<Bill> bills = selectedBankAccount.GetBills();
		Bill selectedBill;
		if (bills.Count != 0)
		{
			foreach (var bill in bills)
			{
				Console.WriteLine(bill.Name + ":" + bill.Cost);
			}
			Console.WriteLine("Select the bill you want to pay");
			int choice;
			if (int.TryParse(Console.ReadLine(), out choice))
			{
				switch (choice)
				{
					case 1:
						selectedBill = selectedBankAccount.GetBill("1.Electricity");
						if (selectedBankAccount.Balance >= selectedBill.Cost)
						{
							selectedBankAccount.Balance -=selectedBill.Cost;
							Console.WriteLine("Electricity bill paid!");
							selectedBankAccount.DeleteBill(selectedBill);
							//adaugare tranzactie 
							Transaction tr = new Transaction(selectedBill.Name, selectedBill.Cost, selectedBankAccount.Id);
							transactions.Add(tr);
						}
						else
						{
							Console.WriteLine("Not enough funds!");
						}
						break;
					case 2:
						selectedBill = selectedBankAccount.GetBill("2.Gas");
						if (selectedBankAccount.Balance >= selectedBill.Cost)
						{
							selectedBankAccount.Balance -= selectedBill.Cost;
							Console.WriteLine("Gas bill paid!");
							selectedBankAccount.DeleteBill(selectedBill);
							//adaugare tranzactie 
							Transaction tr = new Transaction(selectedBill.Name, selectedBill.Cost, selectedBankAccount.Id);
							transactions.Add(tr);
						}
						else
						{
							Console.WriteLine("Not enough funds!");
						}
						break;
					case 3:
						selectedBill = selectedBankAccount.GetBill("3.Water");
						if (selectedBankAccount.Balance >= selectedBill.Cost)
						{
							selectedBankAccount.Balance -= selectedBill.Cost;
							Console.WriteLine("Water bill paid!");
							selectedBankAccount.DeleteBill(selectedBill);
							//adaugare tranzactie 
							Transaction tr = new Transaction(selectedBill.Name, selectedBill.Cost, selectedBankAccount.Id);
							transactions.Add(tr);

						}
						else
						{
							Console.WriteLine("Not enough funds!");
						}
						break;
				}
			}
		}else
        {
			Console.WriteLine("You have no bills to pay for the moment!");
			return;
        }

	}

	void InsertCard()
	{
		Console.WriteLine("Card inserted, please provide a PIN ");
		var pin = Console.ReadLine();
		selectedBankAccount = null;
		foreach (var bankAccount in accounts)
		{
			if (bankAccount.Id != pin)
				continue;
			selectedBankAccount = bankAccount;
			break;
		}
		if (selectedBankAccount == null)
		{
			throw new BankAccountNotFound();
		}
		if (selectedBankAccount.IsLocked == false)
		{
			bool cardInserted = true;
			while (cardInserted)
			{
				ShowCardOptions();
				int choice;
				if (int.TryParse(Console.ReadLine(), out choice))
				{

					switch (choice)
					{
						case 1:
							WithdrawMoney();
							break;
						case 2:
							DepositMoney();
							break;
						case 3:
							PayBills();
							break;
						case 4:
							ShowBalance();
							break;
						case 5:
							cardInserted=false;
							Console.WriteLine("Card withdrawn");
							break;
						case 6:
							foreach (var tr in transactions)
							{
								if (selectedBankAccount.Id == tr.BankAccount) 
								{ 
								Console.WriteLine(tr.BankAccount + " " + tr.TransactionType + " " + tr.TransactionValue);
								}
                            }
							break;
					}
				}
			}
		}else
        {
			Console.WriteLine("Card is blocked");
			return;
        }

    }
	void BlockCard()
    {
		Console.WriteLine("Card inserted, please provide a PIN ");
		var pin = Console.ReadLine();

		selectedBankAccount = null;
		foreach (var bankAccount in accounts)
		{
			if (bankAccount.Id != pin)
				continue;
			selectedBankAccount = bankAccount;
			break;
		}
		if (selectedBankAccount == null)
		{
			throw new BankAccountNotFound();
		}
		selectedBankAccount.IsLocked = true;
		Console.WriteLine("Card was blocked!");
	}
    public static void Main()
	{
		
		Program myProgram = new Program();
		while (true)
		{
			myProgram.ShowUserOptions();

			var input = Console.ReadLine();
			int choice;
			if (int.TryParse(input, out choice))
			{
				switch (choice)
				{
					case 1:
						try
						{
							myProgram.InsertCard();
						}
						catch (BankAccountNotFound ex)
						{
							Console.WriteLine("Bank account not found");
						}
						break;
					case 2:
						try
						{
							myProgram.BlockCard();
						}
						catch (BankAccountNotFound ex)
						{
							Console.WriteLine("Bank account not found");
						}
						break;
				}
			}
			else
			{
			}
		}
	}


	class BankAccountNotFound : Exception
	{

	}

}
