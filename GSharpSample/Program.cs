using System;
using System.IO;
using System.CodeDom.Compiler;
using GSharp.Base.Cores;
using GSharp.Base.Logics;
using GSharp.Base.Scopes;
using GSharp.Base.Singles;
using GSharp.Base.Objects;
using GSharp.Base.Statements;
using GSharp.Compile;
using GSharp.Runtime;
using GSharp.Extension;

namespace GSharpSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // 코드 생성
            GEntry entry = new GEntry();

            GDefine def = new GDefine("valueA");
            GVariable var = def.GetVariable();
            entry.Append(def);

            GEvent main = new GEvent
            (
                new GCommand
                {
                    MethodName = "Loaded",
                    NamespaceName = "this"
                }
            );

            GSet setValue = new GSet(ref var, new GNumber(5));
            main.Append(setValue);

            GIF ifCheck = new GIF(new GCompare(var, GCompare.ConditionType.GREATER_THEN, new GNumber(3)));

            GCommand printCommand = new GCommand
            {
                MethodName = "WriteLine",
                NamespaceName = "Console"
            };

            ifCheck.Append(new GCall(printCommand, new GObject[] { new GString("값이 3보다 큽니다.") }));
            main.Append(ifCheck);

            entry.Append(main);

            // 생성된 코드 컴파일
            string source = entry.ToSource();
            string resultFile = Path.GetTempFileName();

            GCompiler compile = new GCompiler(source);
            GCompilerResults result = compile.Build(resultFile);
            Console.WriteLine(source);

            // 코드 컴파일 결과 분석
            if (result.IsSuccess)
            {
                // 컴파일 성공
                // 컴파일된 시나리오 실행
                Console.WriteLine("컴파일 성공");
                new GExecutor(resultFile, "GSharp.Scenario.Default").CallMethod("Main");
            }
            else
            {
                // 컴파일 실패
                // 컴파일 오류 출력
                foreach (CompilerError error in result.Results.Errors)
                {
                    Console.WriteLine("컴파일 오류 : " + error.Line + " - " + error.ErrorNumber + ":" + error.ErrorText);
                }
            }

            Console.ReadLine();
        }
    }
}
