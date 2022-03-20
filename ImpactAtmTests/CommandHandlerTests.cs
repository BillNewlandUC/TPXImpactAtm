using ImpactAtm;
using NUnit.Framework;
using System;

namespace ImpactAtmTests;

public class CommandHandlerTests
{
    
    [Test]
    public void Example_Success()
    {
        var session = new AccountSession();
        var atm = new Atm();

        var handler = new CommandHandler(new MockResolver(session,atm));
        var l1 = handler.ParseAndExecute("8000");
        var l2 = handler.ParseAndExecute(string.Empty);
        var l3 = handler.ParseAndExecute("12345678 1234 1234");
        var l4 = handler.ParseAndExecute("500 100");
        var l5 = handler.ParseAndExecute("B");
        var l6 = handler.ParseAndExecute("W 100");
        var l7 = handler.ParseAndExecute(string.Empty);
        var l8 = handler.ParseAndExecute("87654321 4321 4321");
        var l9 = handler.ParseAndExecute("100 0");
        var l10 = handler.ParseAndExecute("W 10");
        var l11 = handler.ParseAndExecute(string.Empty);
        var l12 = handler.ParseAndExecute("87654321 4321 4321");
        var l13 = handler.ParseAndExecute("0 0");
        var l14 = handler.ParseAndExecute("W 10");
        var l15 =handler.ParseAndExecute("B");


        Assert.Multiple(() =>
        {
            Assert.IsNull(l1);
            Assert.IsNull(l2);
            Assert.IsNull(l3);
            Assert.IsNull(l4);
            Assert.AreEqual(500,l5);
            Assert.AreEqual(400,l6);
            Assert.IsNull(l7);
            Assert.IsNull(l8);
            Assert.IsNull(l9);
            Assert.AreEqual(90,l10);
            Assert.IsNull(l1);
            Assert.IsNull(l2);
            Assert.IsNull(l3);
            Assert.AreEqual(ErrorStrings.InsufficientFunds,l14);
            Assert.AreEqual(0,l15);
        });
    }

    [Test]
    public void Second_Account_Incorrect_PIN()
    {
        var session = new AccountSession();
        var atm = new Atm();

        var handler = new CommandHandler(new MockResolver(session, atm));
        var l1 = handler.ParseAndExecute("8000");
        var l2 = handler.ParseAndExecute(string.Empty);
        var l3 = handler.ParseAndExecute("12345678 1234 1234");
        var l4 = handler.ParseAndExecute("500 100");
        var l5 = handler.ParseAndExecute("B");
        var l6 = handler.ParseAndExecute("W 100");
        var l7 = handler.ParseAndExecute(string.Empty);
        var l8 = handler.ParseAndExecute("87654321 4321 999");
        var l9 = handler.ParseAndExecute("100 0");
        var l10 = handler.ParseAndExecute("W 10");
        var l11 = handler.ParseAndExecute("B");


        Assert.Multiple(() =>
        {
            Assert.IsNull(l1);
            Assert.IsNull(l2);
            Assert.IsNull(l3);
            Assert.IsNull(l4);
            Assert.AreEqual(500, l5);
            Assert.AreEqual(400, l6);
            Assert.IsNull(l7);
            Assert.AreEqual("ACCOUNT_ERR", l8);
            Assert.AreEqual("ACCOUNT_ERR", l9);
            Assert.AreEqual("ACCOUNT_ERR", l10);
            Assert.AreEqual("ACCOUNT_ERR", l11);
        });
    }


    [Test]
    public void Second_Account_InSufficient_Funds()
    {
        var session = new AccountSession();
        var atm = new Atm();

        var handler = new CommandHandler(new MockResolver(session, atm));
        var l1 = handler.ParseAndExecute("8000");
        var l2 = handler.ParseAndExecute(string.Empty);
        var l3 = handler.ParseAndExecute("12345678 1234 1234");
        var l4 = handler.ParseAndExecute("500 100");
        var l5 = handler.ParseAndExecute("B");
        var l6 = handler.ParseAndExecute("W 100");
        var l7 = handler.ParseAndExecute(string.Empty);
        var l8 = handler.ParseAndExecute("87654321 4321 4321");
        var l9 = handler.ParseAndExecute("100 0");
        var l10 = handler.ParseAndExecute("W 150");
        var l11 = handler.ParseAndExecute("B");


        Assert.Multiple(() =>
        {
            Assert.IsNull(l1);
            Assert.IsNull(l2);
            Assert.IsNull(l3);
            Assert.IsNull(l4);
            Assert.AreEqual(500, l5);
            Assert.AreEqual(400, l6);
            Assert.IsNull(l7);
            Assert.IsNull(l8);
            Assert.IsNull(l9);
            Assert.AreEqual("FUNDS_ERR", l10);
            Assert.AreEqual(100, l11);
        });
    }

    [Test]
    public void Second_Account_InSufficient_ATM_Funds()
    {
        var session = new AccountSession();
        var atm = new Atm();

        var handler = new CommandHandler(new MockResolver(session, atm));
        var l1 = handler.ParseAndExecute("100");
        var l2 = handler.ParseAndExecute(string.Empty);
        var l3 = handler.ParseAndExecute("12345678 1234 1234");
        var l4 = handler.ParseAndExecute("500 100");
        var l5 = handler.ParseAndExecute("B");
        var l6 = handler.ParseAndExecute("W 100");
        var l7 = handler.ParseAndExecute(string.Empty);
        var l8 = handler.ParseAndExecute("87654321 4321 4321");
        var l9 = handler.ParseAndExecute("100 0");
        var l10 = handler.ParseAndExecute("W 50");
        var l11 = handler.ParseAndExecute("B");


        Assert.Multiple(() =>
        {
            Assert.IsNull(l1);
            Assert.IsNull(l2);
            Assert.IsNull(l3);
            Assert.IsNull(l4);
            Assert.AreEqual(500, l5);
            Assert.AreEqual(400, l6);
            Assert.IsNull(l7);
            Assert.IsNull(l8);
            Assert.IsNull(l9);
            Assert.AreEqual("ATM_ERR", l10);
            Assert.AreEqual(100, l11);
        });
    }
    [Test]
    public void Second_Account_Uses_Overdraft()
    {
        var session = new AccountSession();
        var atm = new Atm();

        var handler = new CommandHandler(new MockResolver(session, atm));
        var l1 = handler.ParseAndExecute("8000");
        var l2 = handler.ParseAndExecute(string.Empty);
        var l3 = handler.ParseAndExecute("12345678 1234 1234");
        var l4 = handler.ParseAndExecute("500 100");
        var l5 = handler.ParseAndExecute("B");
        var l6 = handler.ParseAndExecute("W 100");
        var l7 = handler.ParseAndExecute(string.Empty);
        var l8 = handler.ParseAndExecute("87654321 4321 4321");
        var l9 = handler.ParseAndExecute("100 200");
        var l10 = handler.ParseAndExecute("W 150");
        var l11 = handler.ParseAndExecute("B");


        Assert.Multiple(() =>
        {
            Assert.IsNull(l1);
            Assert.IsNull(l2);
            Assert.IsNull(l3);
            Assert.IsNull(l4);
            Assert.AreEqual(500, l5);
            Assert.AreEqual(400, l6);
            Assert.IsNull(l7);
            Assert.IsNull(l8);
            Assert.IsNull(l9);
            Assert.AreEqual(-50, l10);
            Assert.AreEqual(-50, l11);
        });
    }

    public class MockResolver : ICommandResolver
    {
        private readonly IAccountSession _session;
        private readonly IAtm _atm;
        public MockResolver(IAccountSession session, IAtm atm)
        {
            _session = session;
            _atm = atm;
        }
        public ICommand GetCommand(string commandName)
        {
            var command = (ICommand)(commandName.ToUpper() switch
            {
                "A" => new AtmStartCommand(_atm),
                "B" => new BalanceEnquiryCommand(_session),
                "Z" => new EndSessionCommand(_session),
                "N" => new NewSessionCommand(_session),
                "F" => new SetFundsCommand(_session),
                "W" => new WithdrawCommand(_session,_atm),
                _ => throw new ArgumentOutOfRangeException(nameof(commandName), commandName, null)
            });
            if (command == null)
                throw new UnknownCommandException(commandName);
            return command;
        }
    }
}

