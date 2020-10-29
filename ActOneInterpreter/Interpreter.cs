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
            if(node.op.type == Token.PLUS)
                return Visit(node.left) + Visit(node.right);

            if (node.op.type == Token.MINUS)
                return Visit(node.left) - Visit(node.right);

            if (node.op.type == Token.MUL)
                return Visit(node.left) * Visit(node.right);

            if (node.op.type == Token.DIV)
                return Visit(node.left) * Visit(node.right);

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

        public string Interpret() {
            var tree = parser.Parse();
            
            return Visit(tree).ToString();
        }
 
    }
}