using System;
using System.Collections.Generic;

// a. Transaction record
public record Transaction(
    int Id,
    DateTime Date,
    decimal Amount,
    string Category
);

// b. Interface
public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

// c. Transaction processors

public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine(
            $"Bank Transfer: GH₵{transaction.Amount:F2} processed for {transaction.Category}."
        );
    }
}

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine(
            $"Mobile Money: GH₵{transaction.Amount:F2} processed for {transaction.Category}."
        );
    }
}

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine(
            $"Crypto Wallet: GH₵{transaction.Amount:F2} processed for {transaction.Category}."
        );
    }
}

// d. Base Account class
public class Account
{
    public string AccountNumber { get; set; }

    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
    }
}

// e. Sealed SavingsAccount
public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance)
        : base(accountNumber, initialBalance)
    {
    }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds");
        }
        else
        {
            Balance -= transaction.Amount;

            Console.WriteLine(
                $"Transaction of GH₵{transaction.Amount:F2} applied."
            );

            Console.WriteLine(
                $"Updated Balance: GH₵{Balance:F2}"
            );
        }
    }
}

// f. FinanceApp
public class FinanceApp
{
    private List<Transaction> _transactions = new List<Transaction>();

    public void Run()
    {
        SavingsAccount account =
            new SavingsAccount("ACC-1001", 1000m);

        Transaction transaction1 = new Transaction(
            1,
            DateTime.Now,
            150m,
            "Groceries"
        );

        Transaction transaction2 = new Transaction(
            2,
            DateTime.Now,
            200m,
            "Utilities"
        );

        Transaction transaction3 = new Transaction(
            3,
            DateTime.Now,
            100m,
            "Entertainment"
        );

        MobileMoneyProcessor mobileMoney =
            new MobileMoneyProcessor();

        BankTransferProcessor bankTransfer =
            new BankTransferProcessor();

        CryptoWalletProcessor cryptoWallet =
            new CryptoWalletProcessor();

        Console.WriteLine("=== FINANCE MANAGEMENT SYSTEM ===");

        mobileMoney.Process(transaction1);
        account.ApplyTransaction(transaction1);
        _transactions.Add(transaction1);

        Console.WriteLine();

        bankTransfer.Process(transaction2);
        account.ApplyTransaction(transaction2);
        _transactions.Add(transaction2);

        Console.WriteLine();

        cryptoWallet.Process(transaction3);
        account.ApplyTransaction(transaction3);
        _transactions.Add(transaction3);

        Console.WriteLine();
        Console.WriteLine($"Account Number: {account.AccountNumber}");
        Console.WriteLine($"Final Balance: GH₵{account.Balance:F2}");
    }
}

// Main
public class Program
{
    public static void Main()
    {
        FinanceApp app = new FinanceApp();
        app.Run();
    }
}
