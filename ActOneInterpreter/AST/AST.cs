using System;
using System.Collections.Generic;
using System.Text;

namespace ActOneInterpreter {
    //ASTs represent the operator-operand model.
    abstract class AST {
    }

    //node binary operator: an operator that operates on two operands
    class BinOp : AST {
        public readonly Token op;
        public readonly AST left, right;
        public BinOp(AST left, Token op, AST right) {
            this.left = left;
            this.right = right;
            this.op = op;
        }
    }

    class Num : AST {
        public readonly Token token;
        public readonly dynamic value;

        public Num(Token token) {
            this.token = token;
            this.value = token.value;
        }
    }

    class UnaryOp : AST {
        public readonly Token op;
        public readonly AST expr;

        public UnaryOp(Token op, AST right) {
            this.expr = right;
            this.op = op;
        }
    }

    class Compound : AST {
        public readonly AST[] statements;

        public Compound(AST[] statements) {
            this.statements = statements;
        }
    }

    class Assign : AST {
        public readonly Token op;
        public readonly AST left;
        public readonly AST right;
        public Assign(AST left, Token op, AST right) {
            this.left = left;
            this.right = right;
            this.op = op;
        }
    }

    class Var : AST {
        public readonly Token token;
        public readonly dynamic value;

        public Var(Token token) {
            this.token = token;
            this.value = token.value;
        }
    }

    class NoOp : AST {
       
    }
}
