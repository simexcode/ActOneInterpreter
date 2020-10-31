using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.RegularExpressions;

namespace ActOneInterpreter {
    class Interpreter {
        private readonly Parser parser;

        //this is our hacky symbol table aka abstract data type (ADT)
        private Dictionary<string, dynamic> GLOBAL_SCOPE = new Dictionary<string, dynamic>();

        public Interpreter(string programText) {
            parser = new Parser(programText);
        }

        private void Error() {
            throw new Exception("Error parsing input - Interpreter Error");
        }

        private int Visit(AST node) {          
            var methodName = "Visit" + node.GetType().Name;
            MethodInfo theMethod = this.GetType().GetMethod(methodName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (theMethod == null) {
                throw new Exception("No visit method for Node type: " + methodName);
            }
        
            return (int)theMethod.Invoke(this, new object[] { node });            
        }

        private int VisitBinOp(BinOp node) {
            if (node.op.type == Token.PLUS) {
                var left = Visit(node.left);
                var right = Visit(node.right);
                return left + right;
            }

            if (node.op.type == Token.MINUS) {
                var left = Visit(node.left);
                var right = Visit(node.right);
                return left - right;
            }

            if (node.op.type == Token.MUL) {
                var left = Visit(node.left);
                var right = Visit(node.right);
                return left * right;
            }

            if (node.op.type == Token.DIV) {
                var left = Visit(node.left);
                var right = Visit(node.right);
                return left / right;
            }

            Error();
            return -1;
        }

        private int VisitUnaryOp(UnaryOp node) {
            if (node.op.type == Token.PLUS)
                return + Visit(node.expr);

            if (node.op.type == Token.MINUS)
                return -Visit(node.expr);

            Error();
            return -1;
        }

        private int VisitNum(Num node) {
            return node.value;
        }

        private int VisitVar(Var node) {
            var value = GLOBAL_SCOPE[node.value];
            return value;
        }

        private int VisitAssign(Assign node) {
            var name = (node.left as Var).value;
            GLOBAL_SCOPE[name] = Visit(node.right);
            return 0;
        }

        private int VisitCompound(Compound node) {
            foreach (var statement in node.statements) {
                Visit(statement);
            }
            return 0;
        }

        private int VisitNoOp(NoOp node) {
            return 0;
        }

        public Dictionary<string, dynamic> Interpret() {
            var tree = parser.Parse();
            Visit(tree);
            return GLOBAL_SCOPE;
        }
 
    }
}