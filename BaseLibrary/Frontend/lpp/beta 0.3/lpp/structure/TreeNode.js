(function() {
  var TreeNode;

  TreeNode = (function() {
    var _recurse, _setLevel;

    function TreeNode(parentNode, children) {
      var child, i, _i, _len, _ref;

      this.parentNode = parentNode != null ? parentNode : null;
      this.children = children != null ? children : [];
      this.level = (parentNode != null ? parentNode.level : void 0) != null ? parentNode.level + 1 : 0;
      this.index = (parentNode != null ? parentNode.children : void 0) != null ? parentNode.children.length : 0;
      if (parentNode != null) {
        if ((_ref = parentNode.children) != null) {
          _ref.push(this);
        }
      }
      if (lpp.isArray(children)) {
        for (i = _i = 0, _len = children.length; _i < _len; i = ++_i) {
          child = children[i];
          child.parentNode = this;
          child.index = i;
          _recurse(child, _setLevel);
        }
      }
    }

    /* 实例方法
    */


    TreeNode.prototype.appendTo = function(parentNode) {
      this.parentNode = parentNode;
      this.index = parentNode.children.length;
      _recurse(this, _setLevel);
      return void 0;
    };

    TreeNode.prototype.append = function(childNode) {
      var _ref;

      if ((_ref = this.children) == null) {
        this.children = [];
      }
      childNode.index = this.children.length;
      this.children.push(childNode);
      childNode.parentNode = this;
      _recurse(childNode, _setLevel);
      return void 0;
    };

    TreeNode.prototype.getChild = function(index) {
      var _ref;

      if (!((((_ref = this.children) != null ? _ref.length : void 0) > 0) && (this.children.length - 1 > index))) {
        return null;
      }
      return this.children[index];
    };

    TreeNode.prototype.getNextSlibing = function() {
      var _ref, _ref1;

      if (!(((_ref = this.parentNode) != null ? (_ref1 = _ref.children) != null ? _ref1.length : void 0 : void 0) - 1 > this.index)) {
        return null;
      }
      return this.parentNode.children[this.index + 1];
    };

    TreeNode.prototype.getPreSlibing = function() {
      if (!((this.parentNode != null) && this.index > 0)) {
        return null;
      }
      return this.parentNode.children[this.index - 1];
    };

    TreeNode.prototype.getNextPSlibing = function() {
      var _ref, _ref1, _ref2;

      if (!(((_ref = this.parentNode) != null ? (_ref1 = _ref.parentNode) != null ? (_ref2 = _ref1.children) != null ? _ref2.length : void 0 : void 0 : void 0) - 1 > this.parentNode.index)) {
        return null;
      }
      return this.parentNode.parentNode.children[this.parentNode.index + 1];
    };

    TreeNode.prototype.getPrePSlibing = function() {
      var _ref;

      if (!((((_ref = this.parentNode) != null ? _ref.parentNode : void 0) != null) && this.parentNode.index > 0)) {
        return null;
      }
      return this.parentNode.parentNode.children[this.parentNode.index - 1];
    };

    _recurse = function(node, callback) {
      var child, _i, _len, _ref;

      if (typeof callback === "function") {
        callback(node);
      }
      _ref = node.children;
      for (_i = 0, _len = _ref.length; _i < _len; _i++) {
        child = _ref[_i];
        _recurse(child, callback);
      }
      return void 0;
    };

    _setLevel = function(node) {
      return node.level = node.parentNode.level + 1;
    };

    return TreeNode;

  })();

  lpp.define('lpp.structure.TreeNode', TreeNode);

}).call(this);
