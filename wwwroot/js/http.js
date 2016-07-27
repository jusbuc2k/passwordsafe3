define(['exports', 'jquery'], function(exports, jquery) {
    'use strict';

    exports.post = function(url, data) {
        return jquery.ajax({
            method: "post",
            url: url,
            data: data
        });
    };

    exports.get = function(url) {
        return jquery.ajax({
            method: "get",
            url: url,
            dataType: "json"
        });
    };
  
});