# 微信小程序使用示例

![引用结构](https://github.com/DeyiXu/TigerWebApi/raw/master/images/wechat-util.png)

修改文件Constants.js中的参数

```js
const obj = {
  tigerAppID: '10000',
  tigerAppSecret: 'qwerasdfzxcv',
  tigerApiUrl:'http://localhost:53613/rest'
}
```

```js
// 引用包
let client = require('./utils/tigerWebApi/Client.js');
// 参数
let obj = {
      v:'V1',
      uname:'root',
      pass:'pass'
    };
// api名称,请求参数对象,请求成功回调函数
client.Execute('tiger.account.login', obj,function(res){
    console.log(res.data);
});
```