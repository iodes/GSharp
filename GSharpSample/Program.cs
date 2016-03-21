using System;
using System.IO;
using System.CodeDom.Compiler;
using GSharp.Base.Cores;
using GSharp.Base.Logics;
using GSharp.Base.Scopes;
using GSharp.Base.Singles;
using GSharp.Base.Objects;
using GSharp.Base.Methods;
using GSharp.Base.Statements;
using GSharp.Compile;
using GSharp.Runtime;

namespace GSharpSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // 코드 생성
            GBlank root = new GBlank();
            GVoid main = new GVoid("Main");

            GDefine value = new GDefine("valueA");
            GVariable valueVariable = value.GetVariable();
            root.Append(value);

            GSet setValue = new GSet(ref valueVariable, new GNumber(5));
            main.Append(setValue);

            GIF ifCheck = new GIF(new GCompare(valueVariable, new GNumber(3), GCompare.ConditionType.GREATER_THEN));
            ifCheck.Append(new GCall(new GPrint(), new GObject[] { new GString("값이 3보다 큽니다.") }));
            main.Append(ifCheck);

            root.Append(main);

            // 생성된 코드 컴파일
            string source = root.ToSource();
            string resultFile = Path.GetTempFileName();

            GCompiler compile = new GCompiler(root.ToSource());
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
