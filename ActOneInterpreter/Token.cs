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
            BEGIN = "{",
            END = "}",
            SMI = ";",
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


/*
 # Token types
#
# EOF (end-of-file) token is used to indicate that
# there is no more input left for lexical analysis
INTEGER, PLUS, EOF = 'INTEGER', 'PLUS', 'EOF'


class Token(object):
    def __init__(self, type, value):
        # token type: INTEGER, PLUS, or EOF
        self.type = type
        # token value: 0, 1, 2. 3, 4, 5, 6, 7, 8, 9, '+', or None
        self.value = value

    def __str__(self):
        """String representation of the class instance.

        Examples:
            Token(INTEGER, 3)
            Token(PLUS '+')
        """
        return 'Token({type}, {value})'.format(
            type=self.type,
            value=repr(self.value)
        )

    def __repr__(self):
        return self.__str__()
 */