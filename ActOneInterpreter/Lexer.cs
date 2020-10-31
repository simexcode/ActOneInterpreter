using System;
using System.Collections.Generic;
using System.Text;

namespace ActOneInterpreter {
    class Lexer {
        public readonly string programText;             //the program we need to intrepret
        private int position = 0;                       //the current position in the program text
        private char currentCharacter = '\0';           //the current character we are evaluating
        private readonly Dictionary<string, Token> RESERVED_KEYWORDS;

        public Lexer(string programText) {
            this.programText = programText;
            currentCharacter = programText[position];

            RESERVED_KEYWORDS = new Dictionary<string, Token>() {
                { Token.BEGIN, new Token(Token.BEGIN, Token.BEGIN )},
                { Token.END, new Token(Token.END, Token.END )},
            };
        }

        private void Error() {
            throw new Exception("Error parsing input - Lexical Error");
        }

        private char Advance(int number = 1) {
            position += number;

            //if we have reached the end of the file, return a EOF character
            if (position > programText.Length - 1)
                currentCharacter = '\0';
            else
                currentCharacter = programText[position];   //update the current character

            return currentCharacter;
        }

        private char Peek() {
            var character = '\0';
            
            if ((position + 1) <= programText.Length - 1)
                character = programText[position+1];

            return character;
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

        private Token id() {
            var result = "";

            while (currentCharacter != '\0' && char.IsLetterOrDigit(currentCharacter)) {
                result += currentCharacter;
                Advance();
            }

            if (RESERVED_KEYWORDS.ContainsKey(result) == false)
                RESERVED_KEYWORDS[result] = new Token(Token.ID, result);


            return RESERVED_KEYWORDS[result];
        }

        public Token GetNextToken() {
            //Lexical analyzer (also known as scanner or tokenizer)
            //This method is responsible for breaking a sentence apart into tokens. One token at a time.

            var text = this.programText;
            
            SkipWhiteSpace();

            //if our position is grater than the length of the program text, return a new EndOfFile token.
            if (position > text.Length - 1 || currentCharacter == '\0') {
                return new Token(Token.EOF, null);
            }


            //if it starts with a letter we assume it's an identifer token
            if (char.IsLetter(currentCharacter))
                return id();

            if(currentCharacter == ':' && Peek() == '=') {
                Advance(2);
                return new Token(Token.ASSIGN, ":=");
            }

            if (currentCharacter == ';') {
                Advance();
                return new Token(Token.SEMI, Token.SEMI);
            }

            if (currentCharacter == '.') {
                Advance();
                return new Token(Token.DOT, Token.DOT);
            }

            //if the current character is a digit, create an integer token
            if (char.IsDigit(currentCharacter)) {
                return new Token(Token.INTEGER, ParseInteger());
            }

            if (currentCharacter == '+') {
                Advance();
                return new Token(Token.PLUS, currentCharacter); ;
            }

            if (currentCharacter == '-') {
                Advance();
                return new Token(Token.MINUS, currentCharacter); ;
            }

            if (currentCharacter == '*') {
                Advance();
                return new Token(Token.MUL, currentCharacter); ;
            }

            if (currentCharacter == '/') {
                Advance();
                return new Token(Token.DIV, currentCharacter); ;
            }

            if (currentCharacter == '(') {
                Advance();
                return new Token(Token.LPAREN, currentCharacter); ;
            }

            if (currentCharacter == ')') {
                Advance();
                return new Token(Token.RPAREN, currentCharacter); ;
            }

            Error();
            return null;
        }

    }
}