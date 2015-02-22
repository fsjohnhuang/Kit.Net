Ext.define("lppExt.util.Operation", {
    statics: {
        recurseConfig: function (host, actions, callback) {
            var resultActions = [],
                i,
                addingItem;
            for (i = 0; i < actions.length; ++i) {
                addingItem = callback(host, actions[i])
                if (addingItem) {
                    resultActions.push(addingItem);
                }
            }

            return resultActions;
        },
        /* （外部使用）修改actions配置项(actions格式为[{},{},.......])
        ** toolbar {Object} actions配置项
        ** type {String} 工具名
        ** property {String/Object} 属性名；当类型为Object时，即一次性设置多个属性
        ** value {String/Number/Boolean} 属性值；当property为String时有效
        ** typeProperty {String} 入参type对应的属性名称，默认为type
        ** return {Boolean} true修改成功；false入参有误或没有对应的修改项
        */
        update: function (actions, type, property, value, typeProperty) {
            var isSuccess = false;
            if (Ext.isEmpty(actions)) return isSuccess;

            if (Ext.isEmpty(type)) {
                console.warn("Configuration type is empty!");
                return isSuccess;
            }

            var _typeProperty = typeProperty || "type",
                _propVals = {},
                _actions = Ext.clone(actions);
            if (Ext.isObject(property)) {
                _propVals = property;
            }
            else if (Ext.isString(property) && Ext.isDefined(value)) {
                _propVals[property] = value;
            }
            else {
                console.warn("Configuration property or value is wrong!");
                return isSuccess;
            }

            var i, action;
            for (i = 0; i < _actions.length && !isSuccess; ++i) {
                action = _actions[i];
                if (type === action[_typeProperty]) {
                    isSuccess = true;
                }
            }

            if (isSuccess) {
                Ext.apply(action, _propVals);
            }

            return _actions;
        },
        remove: function (actions, type, typeProperty) {
            var removedItem = null;
            if (!Ext.isArray(actions)) {
                console.warn("Configuration actions should be an Array!");
                return removedItem;
            }
            if (Ext.isEmpty(type)) {
                console.warn("Configuration type is empty!");
                return removedItem;
            }

            var i = null,
                _typeProperty = typeProperty || "type",
                _actions = Ext.clone(actions);
            for (var i = 0; i < _actions.length; ++i) {
                if (_actions[i][_typeProperty] === type) {
                    removedItem = _actions[i];
                    _actions.splice(i, 1);
                    break;
                }
            }

            return _actions;
        }
    }
});