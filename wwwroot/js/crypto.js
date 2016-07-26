define(['exports', 'asmcrypto'], function(exports, asmcrypto) {
    'use strict';

    exports.randomKey = function(len) {
        var key = new Uint8Array(len);
        window.crypto.getRandomValues(key);
        return key;
    };
    

    exports.keyFromPassword = function(passwordString, saltBytes) {
        return asmcrypto.PBKDF2_HMAC_SHA256.bytes(passwordString, asmcrypto.bytes_to_hex(saltBytes), 1000, 32);
    };

    exports.encrypt = function(keyBytes, payloadBytes, ivBytes) {
        if (!ivBytes) {
            ivBytes = new Uint8Array(16);
            window.crypto.getRandomValues(iv);
        }

        if (typeof keyBytes === 'string'){
            keyBytes = asmcrypto.base64_to_bytes(keyBytes);
        }

        var cryptoBytes = asmcrypto.AES_CBC.encrypt(payloadBytes, keyBytes, false, ivBytes);
        var result = new Uint8Array(ivBytes.length + cryptoBytes.length);       

        result.set(ivBytes);
        result.set(cryptoBytes, ivBytes.length);

        return asmcrypto.bytes_to_base64(result);
    };

    exports.decrypt = function(keyBytes, cryptoBytes) {
        if (typeof keyBytes === 'string') {
            keyBytes = asmcrypto.base64_to_bytes(keyBytes);
        }

        if (typeof cryptoBytes === 'string'){
            cryptoBytes = asmcrypto.base64_to_bytes(cryptoBytes);
        }

        return asmcrypto.AES_CBC.decrypt(cryptoBytes.slice(16), keyBytes, false, cryptoBytes.slice(0,15));
    };

    exports.extractIV = function(cryptoBytes){
        if (typeof cryptoBytes === 'string'){
            cryptoBytes = asmcrypto.base64_to_bytes(cryptoBytes);
        }

        return cryptoBytes.slice(0.15);
    }

});