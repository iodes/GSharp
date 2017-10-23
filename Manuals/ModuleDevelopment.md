**모듈 제작 방법**
===================

GSharp에 모듈을 제작할 수 있는 방법에 대하여 설명합니다.

아래 사이트에서 모듈에 대해서 따로 관리를 합니다.

[GSharp.Moudules GitHub](https://github.com/iodes/GSharp.Modules/tree/0c5ab28005f96b5d8e14db1e2919424bf51493af)

**※ 이 문서는 Visual Studio 2017기준으로 작성하였습니다.**

**※ 모듈 이름은 일괄성 있게 GSharp.Modules.이름으로 짓어주세요.**

----------

프로젝트 생성
-------------
Visual Studio에서 파일(F) -> 새로 만들기(N) -> 프로젝트(P) -> 클래스 라이브러리(.NET Framework)

![프로젝트 생성 방법](https://i.imgur.com/EVjtT64.png)

----------

모듈 개발 환경 세팅
-------------

모듈을 개발하기 위해서는 참조 관리자(찾아보기)에서 GSharp.Extension.dll를 참조해야합니다. 

**※ 대상  .NET Framework 버전은 4.5로 해주셔야 합니다.**

![참조 방법](https://i.imgur.com/XI8gAfN.png)

![Extension.dll 참조 방법](https://i.imgur.com/BOswesj.png)

또한, 모듈에서 System.Windows.Forms에 내용을 사용하기 위해 참조 관리자(어셈블리)에서 System.Windows.Forms를 활성화 해야합니다.

![System.Windows.Form 참조 방법](https://i.imgur.com/4phvnqX.png)

우측 솔루션 탐색기에서 Class1.cs를 Moudle 이름에 맞게 짓어주고 GMoudle를 상속해줍니다.

    public class TestMessagePrint : GMoudle
    {

    }

GMoudle를 상속 받으면 잠재적 주의 사항이 뜨는데 잠재적 주의 사항을 통해  아래 코드를 추가해줍니다.

    using GSharp.Extension.Abstracts;

![잠재적 주의 사항](https://i.imgur.com/PxziIjZ.png)


----------

모듈 개발
-------------
블록에 이름을 정하기 위해서는 아래와 같은 코드를 사용합니다.

    [GCommand("이름")]

 MessageBox를 출력하는 블록을 만들어 보겠습니다.

      [GCommand("테스트 메시지 출력")]
        public static void ShowMsg()
        {
            MessageBox.Show("테스트 테스트");
        }

입력 받아 메시지 출력하는 모듈

       [GCommand("{0} 메시지 출력")]
        public static void ShowMsg(String test)
        {
            MessageBox.Show("테스트 테스트");
        }

긴 코드는 이렇게 쓰고 get으로 보내서 public에서 결과 값만 return 하게 합니다.

    [GCommand("이름")]
        public static string MyName
        {
            get
            {
                return "test";
            }
        }
        
        private static string code
        {
            엄청 엄청 긴 코드
        } 
   

솔루션 빌드(B)를 합니다.

----------

모듈 적용
-------------
GSharp.Moudules.모듈이름의 폴더를 추가하여  Debug 폴더에서 GSharp.Extension.dll, 프로젝트 이름.dll를 넣고 GSharp.Modules.모듈이름.ini를 추가하여 아래 내용을 추가합니다.

    [General]
	Title=파일
	Author=APEACH
	Summary=파일 입출력과 관련된 블럭을 제공합니다.

	[Assembly]
	File=<%LOCAL%>\GSharp.Modules.모듈이름.dll

GSharp.Moudles.모듈이름 폴더를 GSharpSample\bin\Debug에 Extensions폴더를 만들어서 GSharp.Moudles.모듈이름 폴더를 넣어줍니다.

그 후 GSharpSample.exe를 실행하시면 정상적으로 블록이 적용된 것이 보입니다.

![블록적용 모습](https://i.imgur.com/xGRb8SR.png)
