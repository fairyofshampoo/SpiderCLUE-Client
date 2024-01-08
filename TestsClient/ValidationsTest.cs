using Xunit;
using Spider_Clue.Logic;

namespace TestsClient
{
    public class ValidationsTest
    {
        [Fact]
        public void TestIsPasswordValidSuccess()
        {
            bool result = false;
            string passwordText = "@byFairy0fShampoo";

            result = Validations.IsPasswordValid(passwordText);
            Assert.True(result);
        }

        [Fact]
        public void TestIsPasswordValidFail()
        {
            bool result = false;
            string passwordText = "gatito1";

            result = Validations.IsPasswordValid(passwordText);
            Assert.False(result);
        }

        [Fact]
        public void TestIsMessageValidSuccess()
        {
            bool result = false;
            string messageText = "¡Empecemos la partida ya!";

            result = Validations.IsMessageValid(messageText);
            Assert.True(result);
        }

        [Fact]
        public void TestIsMessageValidFail()
        {
            bool result = false;
            string messageText = "En el tranquilo rincón de la galaxia, " +
                "donde las estrellas bailan su eterna danza cósmica, " +
                "emerge una historia entrelazada con hilos de misterio " +
                "y aventura. En un planeta distante, seres de formas " +
                "inimaginables forjan su destino mientras exploran las " +
                "vastedades de lo desconocido. Entre ruinas ancestrales " +
                "y cielos salpicados de nebulosas, surge un héroe inesperado," +
                " con el peso del universo en sus hombros.\r\n\r\nEste" +
                " intrépido aventurero, provisto de una vieja brújula y" +
                " un astrolabio que guiaron a generaciones anteriores, " +
                "se embarca en un viaje épico. Atravesando mundos alienígenas" +
                " y enfrentándose a criaturas de leyenda, descubre secretos " +
                "que desafían la comprensión humana. Cada paso resuena en" +
                " la inmensidad del cosmos, mientras el destino teje sus" +
                " hilos invisibles en torno a este valiente explorador." +
                "\r\n\r\nEntre las estrellas fugaces y los susurros de" +
                " constelaciones olvidadas, se revela una trama enternecedora" +
                " de amistad y sacrificio. La travesía del héroe se convierte" +
                " en una oda a la curiosidad, recordándonos que, incluso en la" +
                " vastedad del universo, cada historia es una constelación única," +
                " brillando con su propio resplandor en la eternidad cósmica.";

            result = Validations.IsMessageValid(messageText);
            Assert.False(result);
        }

        [Fact]
        public void TestIsEmailValidSuccess()
        {
            bool result = false;
            string emailText = "michellemoreno1313@gmail.com";

            result = Validations.IsEmailValid(emailText);
            Assert.True(result);
        }

        [Fact]
        public void TestIsEmailValidFail()
        {
            bool result = false;
            string emailText = "@spiderclue.xx";

            result = Validations.IsEmailValid(emailText);
            Assert.False(result);
        }

        [Fact]
        public void TestIsNameValidSuccess()
        {
            bool result = false;
            string nameText = "Aneth Michelle";

            result = Validations.IsNameValid(nameText);
            Assert.True(result);
        }

        [Fact]
        public void TestIsNameValidFail()
        {
            bool result = false;
            string nameText = "pollitopio1";

            result = Validations.IsNameValid(nameText);
            Assert.False(result);
        }

        [Fact]
        public void TestIsGamertagValidSuccess()
        {
            bool result = false;
            string gamertagText = "Star3oy";

            result = Validations.IsGamerTagValid(gamertagText);
            Assert.True(result);
        }

        [Fact]
        public void TestIsGamertagValidFail()
        {
            bool result = false;
            string gamertagText = " ";

            result = Validations.IsGamerTagValid(gamertagText);
            Assert.False(result);
        }
    }

}