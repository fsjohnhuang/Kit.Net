module("lpp.structure.TreeNode", function () {
    
});

test("test private fields is valid", function () {
    lpp.Loader.setPaths({
        "lpp": "../lpp"
    });
    /*lpp.syncRequire("lpp.structure.TreeNode");

    root = lpp.create("lpp.structure.TreeNode");
    child1 = lpp.create("lpp.structure.TreeNode", root);
    child2 = lpp.create("lpp.structure.TreeNode", root);
    
    equal(root.children.length, 2, "root.children");
    equal(child1.index, 0, "child1.index");
    equal(child2.index, 1, "child2.index");

    equal(root.level, 0, "root.level");
    equal(child1.level, 1, "child1.level");
    equal(child2.level, 1, "child2.level");

    gChild1 = lpp.create("lpp.structure.TreeNode");
    gChild2 = lpp.create("lpp.structure.TreeNode");
    child3 = lpp.create("lpp.structure.TreeNode", root, [gChild1, gChild2]);
    equal(root.children.length, 3, "root.children");
    equal(child3.index, 2, "child3.index");
    equal(root.children.length, 3, "root.children");*/

    //strEl = lpp.create('lpp.tpl.StrEl')
    /*tn1 = lpp.create('lpp.structure.TreeNode');
    tn2 = lpp.create('lpp.structure.TreeNode');
    tn3 = lpp.create('lpp.structure.TreeNode');
    tn2.append(tn3);

    equal(tn1.index, 0, "tn1.index");
    equal(tn2.index, 0, "tn2.index");
    equal(tn3.index, 0, "tn3.index");
    equal(tn1.level, 0, "tn1.level");
    equal(tn2.level, 0, "tn2.level");
    equal(tn3.level, 1, "tn3.level");

    tn2.appendTo(tn1);
    equal(tn1.index, 0, "tn1.index");
    equal(tn2.index, 0, "tn2.index");
    equal(tn3.index, 0, "tn3.index");
    equal(tn1.level, 0, "tn1.level");
    equal(tn2.level, 1, "tn2.level");
    equal(tn3.level, 2, "tn3.level");*/

    //tns = [tn1, tn2];
    //tn4 = lpp.create('lpp.structure.TreeNode', null, tns);
    //equal(tn1.index, 0, "tn1.index");
    //equal(tn2.index, 1, "tn2.index");
    //equal(tn3.index, 0, "tn3.index");
    //equal(tn1.level, 1, "tn1.level");
    //equal(tn2.level, 1, "tn2.level");
    //equal(tn3.level, 2, "tn3.level");

    //test = {
    //    value: 'John'
    //};
    //test.sayHi = function (name) {
    //    console.log("Hi," + name + ";" + this.value);
    //}
    //test.sayBye = function (name) {
    //    console.log("Bye," + name + ";" + this.value);
    //}
    
    //function filter1(name) {
    //    return name != 'Mary';
    //}
    //function filter2(name) {
    //    return name != 'Keny';
    //}

    //lpp.syncRequire("lpp.Fn");
    //var news = lpp.Fn.createInterceptor(/say.*/, [filter1, filter2], test);
    //test.sayHi("s");
    //test.sayBye("Mary");
    window.test = {
        value: 'John'
    };
    test.getVal = function (gf) {
        return this.value + '\'s ' + gf;
    };
    function re(str) {
        if (str == 'John\'s M') {
            return true;
        }
        return false;
    }
    lpp.syncRequire("lpp.Fn");
    lpp.Fn.createReceptor('getVal', re, test);
});