let comm = require('./Constants.js');
let util = require('./util.js');
let sign = require('./Sign.js');

function execute(method, params, callback) {
  wx.request({
    url: comm.tigerApiUrl,
    data: params,
    header: {
      'content-type': 'application/x-www-form-urlencoded',
      'Rest-AppKey': comm.tigerAppID,
      'Rest-SignMethod': 'md5',
      'Rest-PartnerId': 'TigerWebApiClient-WeChat',
      'Rest-Timestamp': util.formatTime(new Date()),
      'Rest-Method': method,
      'Rest-Sign': sign.buildSignMd5(comm.tigerAppSecret, params)
    },
    method: "POST",
    success: callback
  });
}

module.exports = {
  Execute: execute
};