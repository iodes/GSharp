# GSharp
[日本語説明はこちらへ](README.JP.md)  
[Click here for English Description](README.EN.md)

.NET Framework 코드를 시각적으로 작성할 수 있는 통합 프레임워크 입니다.  
본 프레임워크를 활용하여 비주얼 프로그래밍 솔루션등의 개발이 가능합니다.

## 소개
### [GSharp.Base](GSharp.Base)
객체화된 형태로 로직을 작성할 수 있는 코어 라이브러리 입니다.  
객체화되어 작성된 로직을 코드로 변환하고 관리하는 기능을 수행합니다.

### [GSharp.Bootstrap](GSharp.Bootstrap)
프레임워크를 사용하여 설계된 로직의 실행 환경을 제공하는 라이브러리 입니다.  
설계된 로직에서 자동적으로 형변환이 이루어질 수 있도록 추가적인 기능을 수행합니다.

### [GSharp.Compile](GSharp.Compile)
설계된 로직의 컴파일을 수행하는 라이브러리 입니다.  
외부 참조를 자동으로 검색하고 최종 소스를 컴파일하여 실행 파일을 생성합니다.

### [GSharp.Compressor](GSharp.Compressor)
생성된 실행 파일의 압축 및 압축 해제를 수행하는 라이브러리 입니다.  
일반적으로 컴파일 라이브러리와 함께 사용하여 실행 파일의 용량을 줄입니다.

### [GSharp.Extension](GSharp.Extension)
프레임워크의 모듈 개발을 지원하는 라이브러리 입니다.  
모듈 개발에 필요한 추상 클래스와 문법적 지원을 제공합니다.

### [GSharp.Graphic](GSharp.Graphic)
블럭 코딩 그래픽 컨트롤을 제공하는 라이브러리 입니다.  
각종 블럭 및 블럭 편집기 컨트롤이 여기에 포함됩니다.

### [GSharp.Manager](GSharp.Manager)
프레임워크 모듈 관리를 지원하는 라이브러리 입니다.  
확장 모듈을 검색하고 사용할 수 있도록 합니다.

## 문서
* [모듈 개발 방법](Manuals/ModuleDevelopment.md)

## 라이센스
본 프로그램은 자유 소프트웨어로,  
GNU Lesser General Public 3.0에 준수하여 사용 및 재배포 할 수 있습니다.  
각 프로그램에 참조된 라이브러리는 해당 라이브러리의 라이센스를 준수합니다.
