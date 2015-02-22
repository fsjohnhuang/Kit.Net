Ext.define("lppExt.tree.LocalFilter", {
    statics: {
        NOT_LEAF_TAG: "isn't leaf",
        destroyingNodes: [],
        filter: function _filter(lppTree, filterFn, valObj, mask) {
            if (Ext.isEmpty(lppTree.localChildren)) {
                lppTree.getStore().load({
                    callback: function () {
                        lppExt.tree.LocalFilter.recurseTreeNodes(this.getStore().getRootNode(), filterFn, valObj);
                        mask && mask.hide();
                    },
                    scope: lppTree
                });
            }
            else {
                lppExt.tree.LocalFilter.recurseTreeNodes(lppTree.getStore().getRootNode(), filterFn, valObj);
                mask && mask.hide();
            }
        },
        recurseTreeNodes: function _recurseTreeNodes(model, filterFn, valObj) {
            if (model.isLeaf()) {
                return filterFn(model, valObj);
            }

            if (filterFn(model, valObj)) return lppExt.tree.LocalFilter.NOT_LEAF_TAG;

            var isRemain = false,
                childNodes = model.childNodes,
                destroyingNodes = lppExt.tree.LocalFilter.destroyingNodes || [],
                result = null,
                i = null;
            for (i = childNodes.length - 1; i >= 0; --i) {
                result = _recurseTreeNodes(childNodes[i], filterFn, valObj);
                isRemain = isRemain || (result === lppExt.tree.LocalFilter.NOT_LEAF_TAG || result);
                if (result === lppExt.tree.LocalFilter.NOT_LEAF_TAG) {
                    childNodes[i].collapse();
                }
                else if (result) {
                    childNodes[i].expand();
                }
                else {
                    destroyingNodes.push(childNodes[i]);
                }
            }

            if (_recurseTreeNodes !== _recurseTreeNodes.caller) {
                for (var i = destroyingNodes.length - 1; i >= 0; --i) {
                    destroyingNodes[i].remove(false);
                }
                destroyingNodes = [];
            }

            return isRemain;
        }
    }
});