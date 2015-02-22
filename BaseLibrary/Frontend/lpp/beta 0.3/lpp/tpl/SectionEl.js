(function() {
  var SectionEl;

  SectionEl = (function() {
    function SectionEl(parentNode, children, ctx, scope, section) {
      var p, v, _ref;

      this.section = section;
      SectionEl.__super__.constructor.call(this, parentNode, children);
      this.syntaxEl = lpp.create('lpp.tpl.SyntaxEl', 7, ctx, scope);
      _ref = this.syntaxEl;
      for (p in _ref) {
        v = _ref[p];
        if (lpp.hasOwnProperty(this.syntaxEl, p)) {
          this[p] = v;
        }
      }
    }

    return SectionEl;

  })();

  lpp.define('lpp.tpl.SectionEl', SectionEl);

}).call(this);
