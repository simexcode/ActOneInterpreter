using System;
using System.Collections.Generic;
using System.Text;

namespace ActOneInterpreter {
    class Token {
        public const string 
            INTEGER = "INTEGER", 
            PLUS = "PLUS", 
            MINUS = "MINUS", 
            MUL = "MUL", 
            DIV = "DIV",
            LPAREN = "(",
            RPAREN = ")",
            BEGIN = "BEGIN",
            END = "END",
            SEMI = ";",
            ASSIGN = "=",
            DOT = ".",
            ID = "_ID",
            EOF = "EOF";

        public readonly string type;
        public readonly dynamic value;

        public Token(string type, dynamic value) {
            this.type = type;
            this.value = value;
        }

        public string repr() {
            return this.ToString();
        }

        public override string ToString() {
            return String.Format("{0}, {1}", this.type, this.value);
        }
    }
}