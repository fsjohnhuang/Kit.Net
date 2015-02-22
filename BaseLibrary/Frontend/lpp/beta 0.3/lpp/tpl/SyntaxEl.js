(function() {
  var SyntaxEl;

  SyntaxEl = (function() {
    function SyntaxEl(type, ctx, scope) {
      this.type = type != null ? type : 0;
      this.ctx = ctx != null ? ctx : {};
      this.scope = scope != null ? scope : [lpp.create('lpp.tpl.TplFn'), window];
    }

    return SyntaxEl;

  })();

  lpp.define('lpp.tpl.SyntaxEl', SyntaxEl);

}).call(this);
