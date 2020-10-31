using System;

namespace ActOneInterpreter {
    class Program {
        static void Main(string[] args) {

            var text = @"  BEGIN
                                BEGIN
                                    number := 2;
                                    a := number;
                                    b := 10 * a + 10 * number / 4;
                                    c := a - - b
                                END;
                                x := 11;
                            END.
                            "; //Console.ReadLine();
            //Console.WriteLine(text.Length);
            var interpreter = new Interpreter(text);
            try {
                var result = interpreter.Interpret();

                foreach (var key in result.Keys) {
                    Console.WriteLine(key + ": " + result[key]);
                }
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
            }
        }
    }
}