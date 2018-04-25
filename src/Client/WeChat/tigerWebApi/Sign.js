let md5 = require('./md5.js');

function buildSignMd5(secret, params) {
  let query = secret;

  let keys = Object.keys(params).sort();
  for (let i = 0; i < keys.length; i++) {
    query += (keys[i] + params[keys[i]]);
  }

  query += secret;
  console.log(query);
  return md5.hexMd5(query).toUpperCase();
}

function objectArraySort(keyName) {
  return function (objectN, objectM) {
    var valueN = objectN[keyName]
    var valueM = objectM[keyName]
    if (valueN > valueM) return 1
    else if (valueN < valueM) return -1
    else return 0
  }
}

module.exports = {
  buildSignMd5: buildSignMd5
}