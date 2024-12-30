using CardsClasses;
using NSubstitute;
using packageClasses;
using System.IO;
using System.Security.Principal;

namespace UserClasses.Test
{
    public class TestUser
    {
        //These Tests Where the way i learned how to use Unit Testing so thats why some Tests might be weird

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test1()
        {
            Assert.Pass();
        }

        // testet ob die Stats Richtig Ausgegeben werden und ob die Elo richtig berechnet werden
        // should SUCCED
        [Test]
        public void testPrintStats_scenarioWithResult()
        {
            //ARRANGE
            User testedUser = new User();
            testedUser.SetGetWins = 10;
            testedUser.SetGetLose = 5;

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            //ACT
            testedUser.printStats();

            //ASSERT
            var output = stringWriter.ToString().Trim();
            Assert.AreEqual("------------------\r\nWins: 10\r\nLose: 5\r\nElo: 105\r\n------------------", output);
        }

        // testet ob die Stats Richtig Ausgegeben werden und ob die Elo richtig berechnet werden
        // should FAIL
        [Test]
        public void testPrintStats_scenarioShouldFail()
        {
            //ARRANGE
            User testedUser = new User();
            testedUser.SetGetWins = 10;
            testedUser.SetGetLose = 5;

            var stringWriter = new StringWriter();
            Console.SetOut(stringWriter);

            //ACT
            testedUser.printStats();

            //ASSERT
            var output = stringWriter.ToString().Trim();
            Assert.AreEqual("------------------\r\nWins: 10\r\nLose: 5\r\nElo: 110\r\n------------------", output);
        }

        //testet ob user mit leerem Username erstellt werden K�nnen
        // should SUCCED
        [Test]
        public void testCreateUser_scenarioWithEmptyFields()
        {
            // ARRANGE

            string expectedPwd = "";
            string expectedUsername = "";
            int expectedId = 0;

            // ACT
            User user = new User().createUser(expectedPwd, expectedUsername, expectedId);

            // ASSERT
            Assert.AreEqual(expectedPwd, user.SetGetPassword);
            Assert.AreEqual(expectedUsername, user.SetGetUsername);
            Assert.AreEqual(expectedId, user.SetGetId);
        }

        //testet ob user mit leerem Username erstellt werden K�nnen
        // should SUCCED
        [Test]
        public void testCreateUser_scenarioWithNonEmptyFields()
        {
            // ARRANGE

            string expectedPwd = "Max";
            string expectedUsername = "maxi1234";
            int expectedId = 2;

            // ACT
            User user = new User().createUser(expectedPwd, expectedUsername, expectedId);

            // ASSERT
            Assert.AreEqual(expectedPwd, user.SetGetPassword);
            Assert.AreEqual(expectedUsername, user.SetGetUsername);
            Assert.AreEqual(expectedId, user.SetGetId);
        }

        // user login testen mit correcten input und mit MOCKING
        // should SUCCED
        [Test]
        public void testLoginUser_scenarioWithCorrectValues()
        {
            // ARRANGE
            var user = Substitute.For<User>();
            user.SetGetUsername = "max";
            user.SetGetPassword = "hashedpassword";

            user.VerifyPassword(Arg.Any<string>(), Arg.Any<string>()).Returns(true);

            // ACT
            var token = user.loginUser("testuser", "password");

            // ASSERT
            Assert.AreEqual("max-mtcgToken", token);
        }

        // user login testen mit falschen input und mit MOCKING
        // should FAIL
        [Test]
        public void testLoginUser_scenarioWithIncorrectValues()
        {
            // ARRANGE
            var user = Substitute.For<User>();
            user.SetGetUsername = "maximus";
            user.SetGetPassword = "hashedpasswordIncorrect";

            user.VerifyPassword(Arg.Any<string>(), Arg.Any<string>()).Returns(false);

            // ACT
            var token = user.loginUser("testuser", "password"); 

            // ASSERT
            Assert.AreEqual("max-mtcgToken", token);
        }

        // Open Package mit correct values MOCKING (sollte o returnen f�r keine fehler, 1 f�r anzahl an coins left und 4 f�r die anzahl der cards die der user hat)
        // should SUCCEDE
        [Test]
        public void testOpenPackage_scenarioWithEnoughCoinsAndPackages_ShouldAddCardsToStack()
        {
            // ARRANGE
            var packagesMock = Substitute.For<Packages>();
            var cardMock1 = Substitute.For<Cards>();
            var cardMock2 = Substitute.For<Cards>();

            packagesMock.getPack.Returns(new List<Cards> { cardMock1, cardMock2, cardMock1, cardMock2 });

            var packs = new List<Packages> { packagesMock };

            var user = new User();
            user.SetGetCoins = 5; 
            user.SetGetCardsStack = new List<Cards>();

            // ACT
            var result = user.openPackage(packs);

            // ASSERT
            Assert.AreEqual(0, result.Item2); 
            Assert.AreEqual(1, user.SetGetCoins);
            Assert.AreEqual(4, user.SetGetCardsStack.Count);
        }

    }
}