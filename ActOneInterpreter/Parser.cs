using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Text;

namespace ActOneInterpreter {
    class Parser {
        private readonly Lexer lexer;
        private Token currentToken = null;              //the current token instance

        public Parser(string programText) {
            lexer = new Lexer(programText);

            //Set current token to the first token taken from the input
            currentToken = lexer.GetNextToken();
        }

        private void Error() {
            throw new Exception("Error parsing input - Parser Error");
        }

        private void Eat(string tokenType) {
            if (currentToken.type == tokenType)
                currentToken = lexer.GetNextToken();
            else
                Error();
        }

        private AST Program() {
            //program : compound_statement DOT
            var node = CompundStatement();
            Eat(Token.DOT);
            return node;
        }

        private AST CompundStatement() {
            //compound_statement: BEGIN statement_list END

            Eat(Token.BEGIN);
            var nodes = StatementList();
            Eat(Token.END);

            var root = new Compound(nodes.ToArray());
            return root;
        }

        private List<AST> StatementList() {
            //statement_list : statement | statement SEMI statement_list
            var node = Statement();
            var results = new List<AST>();
            results.Add(node);

            while (currentToken.type == Token.SEMI) {
                Eat(Token.SEMI);
                results.Add(Statement());
            }

            if (currentToken.type == Token.ID) {
                Error();
            }

            return results;
        }

        private AST Statement() {
            //statement : compound_statement | assignment_statement | empty
            if (currentToken.type == Token.BEGIN)
                return CompundStatement();

            if (currentToken.type == Token.ID)
                return AssignmentStatement();

            return Empty();
        }

        private AST AssignmentStatement() {
            //assignment_statement : variable ASSIGN expr
            var left = Variable();
            var token = currentToken;
            Eat(Token.ASSIGN);
            var right = Expr();

            var node = new Assign(left, token, right);
            return node;
        }

        private AST Variable() {
            // variable: ID
            var node = new Var(currentToken);
            Eat(Token.ID);
            return node;
        }

        private AST Empty() {
            //Empty operation. eg. { }
            return new NoOp();
        }

        private AST Factor() {

            var token = currentToken;

            //factor : INTEGER
            //------------------
            if (token.type == Token.INTEGER) {
                Eat(Token.INTEGER);
                return new Num(token);
            }

            //factor : LPAREN expr RPAREN
            //-----------------------------
            if (token.type == Token.LPAREN) {
                Eat(Token.LPAREN);
                var node = Expr();
                Eat(Token.RPAREN);
                return node;
            }

            //factor : (PLUS | MINUS) factor
            //--------------------------------
            if (token.type == Token.MINUS) {
                Eat(Token.MINUS);
                return new UnaryOp(token, Factor());
            }

            if (token.type == Token.PLUS) {
                Eat(Token.PLUS);
                return new UnaryOp(token, Factor());
            }

            //factor : variable
            //--------------------------------
            //if (token.type == Token.ID) {
                //Eat(Token.ID);
                return Variable();
            //}

            Error();
            return null;
        }

        private AST GetTerm() {
            //term : factor ((MUL | DIV) factor)*
            //------------------------------------------

            var node = Factor();      // we start with the first factor

            //we can now have 0 or more (MUL | DIV) followed by a new factor
            while (currentToken.type == Token.MUL || currentToken.type == Token.DIV) {
                var token = currentToken;
                if (currentToken.type == Token.MUL) {
                    Eat(Token.MUL); // we found a 'pluss' token, consume it
                    //result *= Factor(); //add the next term to our result
                }
                else {
                    Eat(Token.DIV);
                    //result /= Factor();
                }
                node = new BinOp(node, token, Factor());
            }

            return node;
        }

        private AST Expr() {
            /*
             expr   : term ((PLUS | MINUS) term)*
             term   : factor ((MUL | DIV) factor)*
             factor : INTEGER | LPAREN expr RPAREN
             */


            var node = GetTerm(); //get the starting value

            while (currentToken.type == Token.PLUS || currentToken.type == Token.MINUS) {
                var token = currentToken;
                if (currentToken.type == Token.PLUS) {
                    Eat(Token.PLUS); // we found a 'pluss' token, consume it
                }
                else {
                    Eat(Token.MINUS);
                }

                node = new BinOp(node, token, GetTerm());
            }

            return node;
        }

        public AST Parse() {
            var node = Program();

            if (currentToken.type != Token.EOF) {
                Error();
            }

            return node;
        }
    }
}
