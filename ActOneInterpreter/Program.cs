using System;

namespace ActOneInterpreter {
    class Program {
        static void Main(string[] args) {
            while (true) {
                var text = Console.ReadLine();
                var interpreter = new Interpreter(text);
                try {
                    var result = interpreter.Interpret();
                    Console.WriteLine(result);
                }
                catch (Exception e) {
                    Console.WriteLine(e.Message);
                }
                
            }
        }
    }
}