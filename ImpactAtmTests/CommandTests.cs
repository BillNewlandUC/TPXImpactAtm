using ImpactAtm;
using NUnit.Framework;
using System;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using NuGet.Frameworks;

namespace ImpactAtmTests;

[TestFixture]
public class CommandTests
{
    [Test]
    public void AtmStartCommand_Success()
    {
        const int balance = 500;

        var atm = new Atm();
        var command = new AtmStartCommand(atm);
        string[] args = { balance.ToString() };
        var result = command.Execute(args);
        Assert.Multiple(() =>
        {
            Assert.IsNull(result);
            Assert.AreEqual(balance, atm.Balance);
        });
    }

    [Test]
    public void AtmStartCommand_NotEnoughArguments()
    {
        void CheckFunction()
        {
            var atm = new Atm();
            var command = new AtmStartCommand(atm);
            string[] args = { };
            var result = command.Execute(args);
        }
        Assert.Throws(typeof(ArgumentException), CheckFunction);
    }

    [Test]
    public void BalanceEnquiryCommand_Success()
    {
        const int balance = 100;
        var session = new AccountSession
        {
            IsValidated = true,
            CurrentBalance = balance
        };
        var command = new BalanceEnquiryCommand(session);
        string[] args = { };
        var result = command.Execute(args);
        Assert.AreEqual(balance, result);
    }

    [Test]
    public void EndSessionCommand_Success()
    {
        var session = new AccountSession
        {
            IsValidated = true,
        };
        var command = new EndSessionCommand(session);
        string[] args = { };
        var result = command.Execute(args);
        Assert.Multiple(() =>
        {
            Assert.IsNull(result);
            Assert.IsFalse(session.IsValidated);
        });
    }


    [Test]
    public void NewSessionCommand_Success()
    {
        const string accountNumber = "1234";
        var session = new AccountSession();
        var command = new NewSessionCommand(session);
        string[] args = { accountNumber, "1000", "1000" };
        var result = command.Execute(args);
        Assert.Multiple(() =>
        {
            Assert.IsNull(result);
            Assert.IsTrue(session.IsValidated);
            Assert.AreEqual(accountNumber,session.AccountNumber );
        });
    }

    [Test]
    public void NewSessionCommand_IncorrectPIN()
    {
        var session = new AccountSession();
        var command = new NewSessionCommand(session);
        string[] args = {"1234", "1000", "9999"};
        var result = command.Execute(args);
        Assert.Multiple(() =>
        {
            Assert.AreEqual(ErrorStrings.AccountError, result);
            Assert.IsFalse(session.IsValidated);
        });
    }


    [Test]
    public void NewSessionCommand_OpeningBalance()
    {
        const int balance = 500;
        const int overdraft = 100;
        var session = new AccountSession {IsValidated = true};
        var command = new SetFundsCommand(session);
        string[] args = { balance.ToString(), overdraft.ToString() };
        var result = command.Execute(args);
        Assert.Multiple(() =>
        {
            Assert.IsNull(result);
            Assert.AreEqual(balance, session.CurrentBalance);
        });
    }

    [Test]
    public void NewSessionCommand_OpeningBalance_NotInitialised()
    {
        const int balance = 500;
        const int overdraft = 100;
        var session = new AccountSession();
        var command = new SetFundsCommand(session);
        string[] args = {balance.ToString(), overdraft.ToString()};
        var result = command.Execute(args);
        Assert.Multiple(() =>
        {
            Assert.AreEqual(ErrorStrings.AccountError, result);
            Assert.Zero(session.CurrentBalance);
            Assert.Zero(session.OverdraftLimit);
            Assert.IsFalse(session.IsValidated);
        });
    }

    [Test]
    public void NewSessionCommand_NotEnoughArguments()
    {
        void CheckFunction()
        {
            var session = new AccountSession();
            var command = new NewSessionCommand(session);
            string[] args = { "1234", "1000" };
            var result = command.Execute(args);
        }
        Assert.Throws(typeof(ArgumentException), CheckFunction);
    }

    [Test]
    public void WithdrawCommand_Success()
    {
        const int atmBalance = 1000;
        const int accountBalance = 100;
        const int amount = 50;

        var atm = new Atm {Balance = atmBalance};
        var session = new AccountSession
        {
            IsValidated = true,
            CurrentBalance = accountBalance
        };

        var command = new WithdrawCommand(session, atm);
        string[] args = { amount.ToString() };
        var result = command.Execute(args);

        Assert.Multiple(() =>
        {
            Assert.AreEqual(accountBalance - amount, result);
            Assert.AreEqual(accountBalance - amount, session.CurrentBalance);
            Assert.AreEqual(atmBalance - amount, atm.Balance);
        });
    }

    [Test]
    public void WithdrawCommand_InsufficientAccountFunds()
    {
        const int atmBalance = 1000;
        const int accountBalance = 100;
        const int amount = 150;

        var atm = new Atm { Balance = atmBalance };
        var session = new AccountSession
        {
            IsValidated = true,
            CurrentBalance = accountBalance
        };

        var command = new WithdrawCommand(session, atm);
        string[] args = { amount.ToString() };
        var result = command.Execute(args);

        Assert.Multiple(() =>
        {
            Assert.AreEqual(ErrorStrings.InsufficientFunds, result);
            Assert.AreEqual(accountBalance, session.CurrentBalance);
            Assert.AreEqual(atmBalance, atm.Balance);
        });
    }

    [Test]
    public void WithdrawCommand_InsufficientAtmFunds()
    {
        const int atmBalance = 100;
        const int accountBalance = 500;
        const int amount = 150;

        var atm = new Atm { Balance = atmBalance };
        var session = new AccountSession
        {
            IsValidated = true,
            CurrentBalance = accountBalance
        };


        var command = new WithdrawCommand(session, atm);
        string[] args = { amount.ToString() };
        var result = command.Execute(args);

        Assert.Multiple(() =>
        {
            Assert.AreEqual(ErrorStrings.AtmError, result);
            Assert.AreEqual(accountBalance, session.CurrentBalance);
            Assert.AreEqual(atmBalance, atm.Balance);
        });
    }

    [Test]
    public void WithdrawCommand_NotEnoughArguments()
    {
        void CheckFunction()
        {
            const int atmBalance = 100;
            const int accountBalance = 500;

            var atm = new Atm { Balance = atmBalance };
            var session = new AccountSession
            {
                IsValidated = true,
                CurrentBalance = accountBalance
            };

            var command = new WithdrawCommand(session, atm);
            string[] args = { };
            var result = command.Execute(args);
        }
        Assert.Throws(typeof(ArgumentException), CheckFunction);
    }

    [Test]
    public void SetFundsCommand_Success()
    {
        const int balance = 500;
        const int overdraft = 100;

        var session = new AccountSession {IsValidated = true};
        var command = new SetFundsCommand(session);
        string[] args = { balance.ToString(), overdraft.ToString() };
        var result = command.Execute(args);

        Assert.Multiple(() =>
        {
            Assert.IsNull(result);
            Assert.AreEqual(balance, session.CurrentBalance);
            Assert.AreEqual(overdraft, session.OverdraftLimit);
        });
    }
    
    [Test]
    public void SetFundsCommand_NotInitialised()
    { 
        const int balance = 500;
        const int overdraft = 100;

        var session = new AccountSession();
        var command = new SetFundsCommand(session);
        string[] args = { balance.ToString(), overdraft.ToString() };
        var result = command.Execute(args);

        Assert.Multiple(() =>
        {
            Assert.AreEqual(ErrorStrings.AccountError,result);
            Assert.AreEqual(0, session.CurrentBalance);
            Assert.AreEqual(0, session.OverdraftLimit);
        });
    }

    [Test]
    public void SetFundsCommand_NotEnoughArguments()
    {
        void CheckFunction()
        {
            var session = new AccountSession { IsValidated = true };
            var command = new SetFundsCommand(session);
            string[] args = {  };
            var result = command.Execute(args);
        }
        Assert.Throws(typeof(ArgumentException), CheckFunction);
    }



}