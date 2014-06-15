XiyouNewsAPI
============

西邮新闻网API

西邮新闻网REST API，采用ASP.NET MVC 4开发，用于返回JSON数据，项目中采用了HtmlAgility开源.NET类库，感谢原作者！

西邮新闻网官网：http://news.xupt.edu.cn/

API支持不含图片和视频的模块！

1.新闻列表：
  
  NewsList
  
  参数：type（必选），page（根据第一次请求得到的pages确定）
  
2.新闻详情：
  
  NewsDetail
  
  参数：link（必选，根据新闻列表得到的link字段确定），haspic（可选，是否含图）
