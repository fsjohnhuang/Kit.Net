!function () {
    this.lpp.util = {
        getType: function (obj) {
            var type;
            return ((type = typeof (obj)) == "object" ?
	            obj == null && "null" || Object.prototype.toString.call(obj).slice(8, -1) : type).toLowerCase();
        },
        // types 为string或array
        isType: function (obj, types) {
            var valid = false;
            if (arguments.length !== 2) throw { message: "arguments is wrong!", name: "Exception!" };

            var _types = [];
            if ("array" === this.getType(types)) {
                _types = types;
            }
            else if ("string" === this.getType(types)) {
                _types.push(types);
            }
            else {
                throw { message: "Data type of dataTypes is wrong!", name: "Data Type Exception!" };
            }

            for (var i = _types.length - 1; i >= 0; --i) {
                if (_types[i].toLocaleLowerCase() === this.getType(obj)) {
                    valid = true;
                    break;
                }
            }

            return valid;
        },
        isNothing: function (obj) {
            var type = this.getType(obj);
            return type === "null" || type === "undefined";
        },
        /* 深度复制
	    ** @param {array/object} destination 目标对象
	    ** @param {array/object} source 源对象
	    */
        extend: function _extend(destination,source)
	    {
		    for(var p in source)
		    {
			    if(getType(source[p])=="array"||getType(source[p])=="object")
			    {
				    destination[p]=getType(source[p])=="array"?[]:{};
				    _extend(destination[p], source[p]);
			    }
			    else
			    {
				    destination[p]=source[p];
			    }
		    }
        },
        each: function (arrays, callback) {
            var i, l, item, j;
            for (i = j = 0, l = arrays.length; i < l; j = ++i) {
                if (callback) {
                    callback(j, item);
                }
            }
        },
        validateType: function (obj, dataTypes) {
            var valid = false;
            if (arguments.length !== 2) throw { message: "arguments is wrong!", name: "Exception!" };

            var _dataTypes = [];
            if ("array" === this.getType(dataTypes)) {
                _dataTypes = dataTypes;
            }
            else if ("string" === this.getType(dataTypes)) {
                _dataTypes.push(dataTypes);
            }
            else {
                throw { message: "Data type of dataTypes is wrong!", name: "Data Type Exception!" };
            }

            for (var i = _dataTypes.length - 1; i >= 0; --i) {
                if (_dataTypes[i].toLocaleLowerCase() === this.getType(obj)) {
                    valid = true;
                    break;
                }
            }

            return valid;
        },
        // 生成dom.contains函数，用于判断入参节点是否为该节点的子节点
        contains: function () {
            if (!!window.find) {
                HTMLElement.prototype.contains = function (B) {
                    return this.compareDocumentPosition(B) - 19 > 0
                }
            }
        }
    };
}();