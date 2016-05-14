using System;
using System.IO;
using System.CodeDom.Compiler;
using GSharp.Base.Cores;
using GSharp.Base.Scopes;
using GSharp.Base.Singles;
using GSharp.Base.Objects;
using GSharp.Base.Statements;
using GSharp.Compile;
using GSharp.Extension;
using GSharp.Base.Objects.Numbers;
using GSharp.Base.Objects.Logics;
using GSharp.Base.Objects.Strings;

namespace GSharpSample
{
    class Program
    {
        static void Main(string[] args)
        {
            // 코드 생성
            GEntry entry = new GEntry();

            GDefine def = new GDefine("valueA");
            GNumberVariable var = new GNumberVariable("valueA");
            entry.Append(def);

            GEvent main = new GEvent(new GCommand("this", "Loaded", typeof(void), GCommand.CommandType.Event));
            GSet<GNumberVariable, GNumber> setValue = new GSet<GNumberVariable, GNumber>(var, new GNumberConst(5));
            main.Append(setValue);

            GIf ifCheck = new GIf(new GCompare<GNumber>(var, GCompare<GNumber>.ConditionType.GREATER_THEN, new GNumberConst(3)));
            ifCheck.Append(
                new GVoidCall(
                    new GCommand("Console", "WriteLine", typeof(void), GCommand.CommandType.Call),
                    new GObject[] { new GStringCat(new GStringConst("A"), new GConvertNumberToString(new GNumberConst(5))) }
                )
            );

            main.Append(ifCheck);
            entry.Append(main);

            // 생성된 코드 컴파일
            string source = entry.ToSource();
            string resultFile = Path.GetTempFileName();

            GCompiler compile = new GCompiler(source);
            GCompilerResults result = compile.Build(resultFile);
            Console.WriteLine(result.Source);

            // 코드 컴파일 결과 분석
            if (result.IsSuccess)
            {
                // 컴파일 성공
                // 컴파일된 시나리오 실행
                Console.WriteLine("컴파일 성공");
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
