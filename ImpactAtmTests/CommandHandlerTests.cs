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
        handler.ParseAndExecute("8000");
        handler.ParseAndExecute(string.Empty);
        handler.ParseAndExecute("12345678 1234 1234");
        handler.ParseAndExecute("500 100");
        var l1 = handler.ParseAndExecute("B");
        var l2 = handler.ParseAndExecute("W 100");
        handler.ParseAndExecute(string.Empty);
        handler.ParseAndExecute("87654321 4321 4321");
        handler.ParseAndExecute("100 0");
        var l3 = handler.ParseAndExecute("W 10");
        handler.ParseAndExecute(string.Empty);
        handler.ParseAndExecute("87654321 4321 4321");
        handler.ParseAndExecute("0 0");
        var l4 = handler.ParseAndExecute("W 10");
        var l5 =handler.ParseAndExecute("B");


        Assert.Multiple(() =>
        {
            Assert.AreEqual(500,l1);
            Assert.AreEqual(400,l2);
            Assert.AreEqual(90,l3);
            Assert.AreEqual(ErrorStrings.InsufficientFunds,l4);
            Assert.AreEqual(0,l5);
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

