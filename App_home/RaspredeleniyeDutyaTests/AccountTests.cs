using RaspredeleniyeDutyaApp.Controllers;
using RaspredeleniyeDutyaFormulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RaspredeleniyeDutyaTests
{
    [TestClass]
    public class AccountTests
    {
        private static string? CheckPassword(string password)
            => ServerController.CheckPassword(password, password);

        [TestMethod]
        public void PasswordTest()
        {
            Assert.IsNull(CheckPassword("HelloWorld1092!?"));
            Assert.IsNull(CheckPassword("ImJustAPassword?0@)"));
            Assert.IsNull(CheckPassword("I!Love@Coding?"));

            Assert.IsNotNull(CheckPassword("Invalid password"));
            Assert.IsNotNull(CheckPassword("Short!1"));
            Assert.IsNotNull(CheckPassword("11111111"));
            Assert.IsNotNull(CheckPassword("Abchdjekd"));
            Assert.IsNotNull(CheckPassword("()!@(&%*^$$%^"));
        }

        [TestMethod]
        public void EmailTest()
        {
            Assert.IsTrue(ServerController.IsEmailValid("prettyandsimple@example.com"));
            Assert.IsTrue(ServerController.IsEmailValid("very.common@example.com"));
            Assert.IsTrue(ServerController.IsEmailValid("disposable.style.email.with+symbol@example.com"));
            Assert.IsTrue(ServerController.IsEmailValid("other.email-with-dash@example.com"));
            Assert.IsTrue(ServerController.IsEmailValid("x@example.com"));
            Assert.IsTrue(ServerController.IsEmailValid("\"much.more unusual\"@example.com"));
            Assert.IsTrue(ServerController.IsEmailValid("\"very.unusual.@.unusual.com\"@example.com"));
            Assert.IsTrue(ServerController.IsEmailValid("#!$%&'*+-/=?^_`{}|~@example.org"));
            Assert.IsTrue(ServerController.IsEmailValid("\" \"@example.org"));

            Assert.IsFalse(ServerController.IsEmailValid("invalid email"));
            Assert.IsFalse(ServerController.IsEmailValid("@a.com"));
            Assert.IsFalse(ServerController.IsEmailValid("@@a.com"));
            Assert.IsFalse(ServerController.IsEmailValid("test[]@gmail.com"));
            Assert.IsFalse(ServerController.IsEmailValid("test@test@gmail.com"));
            Assert.IsFalse(ServerController.IsEmailValid("Email inside a string test@gmail.com here"));
            Assert.IsFalse(ServerController.IsEmailValid("Email after a string test@gmail.com"));
            Assert.IsFalse(ServerController.IsEmailValid("test@gmail.com Email before a string"));
            Assert.IsFalse(ServerController.IsEmailValid("Abc.example.com"));
            Assert.IsFalse(ServerController.IsEmailValid("a\"b(c)d,e:f;gi[j\\k]l@example.com"));
            Assert.IsFalse(ServerController.IsEmailValid("just\"not\"right@example.com"));
            Assert.IsFalse(ServerController.IsEmailValid("this is\"not\\allowed@example.com"));
            Assert.IsFalse(ServerController.IsEmailValid("this\\ still\\\"not\\allowed@example.com"));
        }
    }
}
