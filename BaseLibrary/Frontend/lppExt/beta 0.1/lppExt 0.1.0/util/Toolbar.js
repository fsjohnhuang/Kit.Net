Ext.define("lppExt.util.Toolbar", {
    requires: ["lppExt.util.Common"],
    statics: {
        /* (内部使用)遍历lppTree、lppGrid的toolbar配置项
        ** toolbar {Object} toolbar配置项
        ** callback {Function} 对toolbar配置项中的工具项的操作函数
        ** return {Array} dockedItems数组
        */
        recurseConfig: function (host, toolbar, callback) {
            var dockedItems = [], // 结果对象数组
                docks = Ext.Object.getKeys(toolbar),
                l1Items = null, // 第一层数组或对象
                l2Items = null, // 第二层数组或对象
                i = null,
                j = null,
                k = null,
                addingItems = [],
                addintItem = null;
            for (i = 0; i < docks.length; ++i) {
                l1Items = toolbar[docks[i]];
                if (l1Items === null || Ext.isEmpty(l1Items)) continue;
                if (Ext.isObject(l1Items)) {
                    addingItem = callback(host, l1Items);
                    addingItem.dock = docks[i];
                    dockedItems.push(addingItem);
                    continue;
                }
                else if (!Ext.isArray(l1Items)) {
                    continue;
                }

                for (j = 0; j < l1Items.length; ++j) {
                    l2Items = l1Items[j];
                    if (Ext.isArray(l2Items)) {
                        if (Ext.isEmpty(l2Items)) continue;

                        for (k = 0; k < l2Items.length; ++k) {
                            addingItem = callback(host, l2Items[k]);
                            if (addingItem !== null) {
                                addingItems.push(addingItem);
                            }
                        }
                        if (addingItems.length >= 1) {
                            dockedItems.push({
                                xtype: "toolbar",
                                dock: docks[i],
                                items: addingItems
                            });
                        }

                        addingItems = [];
                        addingItem = null;
                    }
                    else if (Ext.isObject(l2Items)) {
                        addingItem = callback(host, l2Items);
                        if (addingItem !== null) {
                            addingItems.push(addingItem);
                        }

                        addingItem = null;
                    }
                }
                // l2Items全为对象时
                if (addingItems.length >= 1) {
                    dockedItems.push({
                        xtype: "toolbar",
                        dock: docks[i],
                        items: addingItems
                    });

                    addingItems = [];
                }
            }

            return dockedItems;
        },
        /* （外部使用）修改toolbar配置项
        ** toolbar {Object} toolbar配置项
        ** type {String} 工具名
        ** property {String/Object} 属性名；当类型为Object时，即一次性设置多个属性
        ** value {String/Number/Boolean} 属性值；当property为String时有效
        ** return {Boolean} true修改成功；false入参有误或没有对应的修改项
        */
        update: function (toolbar, type, property, value) {
            var isSuccess = false;
            if (Ext.isEmpty(toolbar)) return isSuccess;

            if (Ext.isEmpty(type)) {
                console.warn("Configuration type is empty!");
                return isSuccess;
            }

            var _propVals = {};
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

            var docks = Ext.Object.getKeys(toolbar),
                l1Items = null, // 第一层数组
                l2Items = null, // 第二层数组或对象
                targetItem = null, // 目标对象
                i = null,
                j = null,
                k = null,
                _toolbar = Ext.clone(toolbar);
            for (i = 0; i < docks.length && targetItem === null; ++i) {
                l1Items = _toolbar[docks[i]];
                if (l1Items === null || !Ext.isArray(l1Items) || Ext.isEmpty(l1Items)) continue;

                for (j = 0; j < l1Items.length && targetItem === null; ++j) {
                    l2Items = l1Items[j];
                    if (Ext.isArray(l2Items)) {
                        if (Ext.isEmpty(l2Items)) continue;

                        for (k = 0; k < l2Items.length && targetItem === null; ++k) {
                            if (l2Items[k].type === type) {
                                targetItem = l2Items[k];
                            }
                        }
                    }
                    else if (Ext.isObject(l2Items)) {
                        if (l2Items.type === type) {
                            targetItem = l2Items;
                        }
                    }
                }
            }

            if (targetItem) {
                Ext.apply(targetItem, _propVals);
                isSuccess = true;
            }

            return _toolbar;
            //return isSuccess;
        },
        /* （外部使用）插入工具到参考工具前
        ** toolbar {Object} toolbar配置项
        ** type {String} 参考的工具类型
        ** item {Object} 插入工具
        ** return {Boolean} 
        */
        insertBefore: function (toolbar, type, item) {
            var isSuccess = false;
            if (!Ext.isObject(toolbar)) {
                console.warn("Configuration toolbar should be an Object!");
                return isSuccess;
            }
            if (Ext.isEmpty(type)) {
                console.warn("Configuration type is empty!");
                return isSuccess;
            }
            if (Ext.isEmpty(item.type)) {
                console.warn("Configuration item is wrong!");
                return isSuccess;
            }

            var docks = Ext.Object.getKeys(toolbar),
                l1Items = null, // 第一层数组
                l2Items = null, // 第二层数组或对象
                tmpItems = null, // 临时存放工具
                i = null,
                j = null,
                k = null,
                l = null;
            for (i = 0; i < docks.length && !isSuccess; ++i) {
                l1Items = toolbar[docks[i]];
                if (l1Items === null || !Ext.isArray(l1Items) || Ext.isEmpty(l1Items)) continue;

                for (j = 0; j < l1Items.length && !isSuccess; ++j) {
                    l2Items = l1Items[j];
                    if (Ext.isArray(l2Items)) {
                        if (Ext.isEmpty(l2Items)) continue;

                        for (k = 0; k < l2Items.length && !isSuccess; ++k) {
                            if (l2Items[k].type === type) {
                                isSuccess = true;
                                tmpItems = l2Items.splice(k, l2Items.length - k);
                                l2Items.push(item);
                                for (l = 0; l < tmpItems.length; ++l) {
                                    l2Items.push(tmpItems[l]);
                                }
                            }
                        }
                    }
                    else if (Ext.isObject(l2Items)) {
                        if (l2Items.type === type) {
                            isSuccess = true;
                            tmpItems = l1Items.splice(j, l1Items.length - j);
                            l1Items.push(item);
                            for (l = 0; l < tmpItems.length; ++l) {
                                l1Items.push(tmpItems[l]);
                            }
                        }
                    }
                }
            }

            return isSuccess;
        },
        /* （外部使用）插入工具到参考工具后
        ** toolbar {Object} toolbar配置项
        ** type {String} 参考的工具类型
        ** item {Object} 插入工具
        ** return {Boolean} 
        */
        insertAfter: function (toolbar, type, item) {
            var isSuccess = false;
            if (!Ext.isObject(toolbar)) {
                console.warn("Configuration toolbar should be an Object!");
                return isSuccess;
            }
            if (Ext.isEmpty(type)) {
                console.warn("Configuration type is empty!");
                return isSuccess;
            }
            if (Ext.isEmpty(item.type)) {
                console.warn("Configuration item is wrong!");
                return isSuccess;
            }

            var docks = Ext.Object.getKeys(toolbar),
                l1Items = null, // 第一层数组
                l2Items = null, // 第二层数组或对象
                tmpItems = null, // 临时存放工具
                i = null,
                j = null,
                k = null,
                l = null;
            for (i = 0; i < docks.length && !isSuccess; ++i) {
                l1Items = toolbar[docks[i]];
                if (l1Items === null || !Ext.isArray(l1Items) || Ext.isEmpty(l1Items)) continue;

                for (j = 0; j < l1Items.length && !isSuccess; ++j) {
                    l2Items = l1Items[j];
                    if (Ext.isArray(l2Items)) {
                        if (Ext.isEmpty(l2Items)) continue;

                        for (k = 0; k < l2Items.length && !isSuccess; ++k) {
                            if (l2Items[k].type === type) {
                                isSuccess = true;
                                tmpItems = l2Items.splice(k + 1, l2Items.length - k);
                                l2Items.push(item);
                                for (l = 0; l < tmpItems.length; ++l) {
                                    l2Items.push(tmpItems[l]);
                                }
                            }
                        }
                    }
                    else if (Ext.isObject(l2Items)) {
                        if (l2Items.type === type) {
                            isSuccess = true;
                            tmpItems = l1Items.splice(j + 1, l1Items.length - j);
                            l1Items.push(item);
                            for (l = 0; l < tmpItems.length; ++l) {
                                l1Items.push(tmpItems[l]);
                            }
                        }
                    }
                }
            }

            return isSuccess;
        },
        /* （外部使用）删除指定类型的工具
        ** toolbar {Object} toolbar配置项
        ** type {String} 工具类型
        ** return {Object} 被删除的工具；没有被删除的工具时，返回null
        */
        remove: function (toolbar, type) {
            var removedItem = null;
            if (!Ext.isObject(toolbar)) {
                console.warn("Configuration toolbar should be an Object!");
                return removedItem;
            }
            if (Ext.isEmpty(type)) {
                onsole.warn("Configuration type is empty!");
                return removedItem;
            }

            var docks = Ext.Object.getKeys(toolbar),
                l1Items = null, // 第一层数组
                l2Items = null, // 第二层数组或对象
                i = null,
                j = null,
                k = null,
                _toolbar = Ext.clone(toolbar);
            for (i = 0; i < docks.length && removedItem === null; ++i) {
                l1Items = _toolbar[docks[i]];
                if (l1Items === null || !Ext.isArray(l1Items) || Ext.isEmpty(l1Items)) continue;

                for (j = 0; j < l1Items.length && removedItem === null; ++j) {
                    l2Items = l1Items[j];
                    if (Ext.isArray(l2Items)) {
                        if (Ext.isEmpty(l2Items)) continue;

                        for (k = 0; k < l2Items.length && removedItem === null; ++k) {
                            if (l2Items[k].type === type) {
                                removedItem = l2Items[k];
                                l2Items.splice(k, 1);
                            }
                        }
                    }
                    else if (Ext.isObject(l2Items)) {
                        if (l2Items.type === type) {
                            removedItem = l2Items;
                            l1Items.splice(j, 1);
                        }
                    }
                }
            }
            
            return _toolbar;
            //return removedItem;
        },
        /* （外部使用）清空工具栏
        ** toolbar {Object} toolbar配置项
        ** return {void}
        */
        removeAll: function (toolbar) {
            var item = null;
            for (item in toolbar) {
                delete toolbar[item];
            }
        },
        resetToolbarRemotely: function (url, toolbar) {
            lppExt.util.Common.syncRequest({
                scope: this,
                url: url,
                success: lppExt.util.Common.createCallback(function (toolbar, response, eOpt) {
                    var obj = Ext.JSON.decode(response.responseText, true);
                    if (obj === null || !obj.success) return;

                    var i;
                    for (i = 0; i < obj.items.length; ++i) {
                        if (obj.items[i].type) {
                            lppExt.util.Toolbar.update(toolbar, obj.items[i].type, obj.items[i]);
                        }
                    }
                }, toolbar)
            });

            return toolbar;
        }
    }
});