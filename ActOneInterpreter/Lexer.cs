using System;
using System.Collections.Generic;
using System.Text;

namespace ActOneInterpreter {
    class Lexer {
        public readonly string programText;             //the program we need to intrepret
        private int position = 0;                       //the current position in the program text
        private char currentCharacter = '\0';           //the current character we are evaluating

        public Lexer(string programText) {
            this.programText = programText;
            currentCharacter = programText[position];
        }

        private void Error() {
            throw new Exception("Error parsing input - Lexical Error");
        }

        private char Advance() {
            position++;

            //if we have reached the end of the file, return a EOF character
            if (position > programText.Length - 1)
                currentCharacter = '\0';
            else
                currentCharacter = programText[position];   //update the current character

            return currentCharacter;
        }

        private void SkipWhiteSpace() {
            //while we have not reached EOF and we have a white space character advance
            while (currentCharacter != '\0' && char.IsWhiteSpace(currentCharacter)) {
                Advance();
            }
        }

        private int ParseInteger() {
            //Return a (multidigit) integer consumed from the input.
            var result = "";

            while (char.IsDigit(currentCharacter)) {
                result += currentCharacter; //if we have a character symbol, add it to our result
                Advance();                  //get the next symbol
            }

            return int.Parse(result);
        }

        public Token GetNextToken() {
            //Lexical analyzer (also known as scanner or tokenizer)
            //This method is responsible for breaking a sentence apart into tokens. One token at a time.

            var text = this.programText;

            //if our position is grater than the length of the program text, return a new EndOfFile token.
            if (position > text.Length - 1) {
                return new Token(Token.EOF, null);
            }

            //while () {
            SkipWhiteSpace();
            //}

            //get a character at the position self.pos and decide what token to create based on the single character
            char current_char = text[position];

            //if the current character is a digit, create an integer token
            if (char.IsDigit(current_char)) {
                return new Token(Token.INTEGER, ParseInteger());
            }

            if (current_char == '+') {
                Advance();
                return new Token(Token.PLUS, current_char); ;
            }

            if (current_char == '-') {
                Advance();
                return new Token(Token.MINUS, current_char); ;
            }

            if (current_char == '*') {
                Advance();
                return new Token(Token.MUL, current_char); ;
            }

            if (current_char == '/') {
                Advance();
                return new Token(Token.DIV, current_char); ;
            }

            if (current_char == '(') {
                Advance();
                return new Token(Token.LPAREN, current_char); ;
            }

            if (current_char == ')') {
                Advance();
                return new Token(Token.RPAREN, current_char); ;
            }

            Error();
            return null;
        }
    }
}