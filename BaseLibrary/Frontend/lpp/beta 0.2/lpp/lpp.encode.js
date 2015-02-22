!function () {
    this.lpp.encode = {
        toASCII: function (str) {
               return this.ToNormal(str).replace(/[^\u0000-\u00FF]/g, function () { return escape(arguments[0]).replace(/(?:%u)([0-9a-f]{4})/gi, "\$1;") });
        },
        toUnicode: function (str) {
            return this.ToNormal(str).replace(/[^\u0000-\u00FF]/g, function () { return escape(arguments[0]).replace(/(?:%u)([0-9a-f]{4})/gi, "\\u$1") });
        },
        toNormal: function (str) {
            return str.replace(/(?:)([0-9a-f]{4});|(?:\\u)([0-9a-f]{4})/gi, function () { return unescape("%u" + (arguments[1] || arguments[2])); });
        },
        toCss: function (str) {
            return this.ToNormal(str).replace(/[^\u0000-\u00FF]/g, function () { return escape(arguments[0]).replace(/(?:%u)([0-9a-f]{4})/gi, "\\$1") });
        }
    };
}();
