# PDFService
Office格式文件（支持doc、docx、xls、xlsx、ppt、pptx）转换为PDF格式文件、PDF文件签名盖章服务。  
PDF转换使用Office COM组件实现，兼容性好，PDF文件签名盖章基于iText7实现。

### 1. PDF转换
1. 请求地址：/pdf/convert
2. 请求参数  
    file: Office格式文件  
    sign:布尔值，true：签名盖章；false：不签名盖章。默认false  
    regex: 签名标记正则表达式，默认“盖章”
    pages: 标记查找页码，负数倒数页码, 默认-1
    latest: 是否在最后一个标记处签名

    
### 2. PDF签名
1. 请求地址：/pdf/sign
2. 请求参数  
    file: PDF格式文件  
    regex: 签名标记正则表达式，默认“盖章”
    pages: 标记查找页码，负数倒数页码, 默认-1
    latest: 是否在最后一个标记处签名

`接口详见swagger(http://localhost:5000/index.html)`