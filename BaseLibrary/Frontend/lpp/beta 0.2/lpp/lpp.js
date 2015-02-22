!function () {

	var _tool = {};
	_tool.getSingleDom = function (dom) {
		if (dom.length) {
			return dom[0];
		}
		return dom;
	};
	_tool.each = function (obj, fn) {
	    if (!obj.length) return fn(obj);

	    var i;
        for (i = 0; i < obj.length; ++i) {
            fn(obj[i]);
        }
	};
    /* @param propertyName{String} config中的属性名
    ** @param defaultVal 赋予的默认值
    ** @param dataTypes {String/Array} 值类型，若属性值不属于该值类型则抛异常
    ** @param includsion {String/Array} 值包含内容正则表达式，若属性值不符合则抛异常
    */
    _tool.getConfigVal = function (config, propertyName, defaultVal, dataTypes, includsion) {
        var _val = null;
        var _valid = false;
        if (config && config[propertyName] && !lpp.util.isNothing(dataTypes)) {
            _val = config[propertyName];
            _valid = lpp.util.validateType(_val, dataTypes);
            if (!_valid) {
                throw { message: ["Data type of ", propertyName, " is wrong!"].join(), name: "Data Type Exception!" };
            }

            if (dataTypes === "string" && !lpp.util.isNothing(includsion)) {
                var _includsion = [];
                if (typeof includsion === "string") {
                    _includsion.push(includsion);
                }
                else {
                    _includsion = includsion;
                }
                _valid = false;
                for (var i = _includsion.length - 1; i >= 0; --i) {
                    var exp = new RegExp(_includsion[i]);
                    if (exp.test(_val)) {
                        _valid = true;
                        break;
                    }
                }

                if (!_valid) {
                    throw { message: ["Value of ", propertyName, " is wrong!"].join(), name: "Value Range Exception!" };
                }
                return _val;
            }
            else {
                return _val;
            }
        }
        else {
            return (config && typeof config[propertyName] !== 'undefined' ? config[propertyName] : defaultVal);
        }
    };

	// dom为id、tagName、className、dom
	this.lpp = function (dom) {
		return new lppEl(dom);
	};

	function lppEl(dom, pDom) {
	    var _lppEl = {
                children: []
	        },
			_pDom = pDom || document;
		if (typeof dom === 'string' && dom.length >= 1) {
		    if (dom.indexOf('#') === 0){
		        // id
		        _lppEl.dom = _pDom.getElementById(dom.replace('#',''));
		    }
		    else if (dom.indexOf('.') === 0){
		        // className
		        _lppEl.dom = _pDom.getElementsByClassName(dom.replace('.', ''));
		    }
		    else if (dom.indexOf("<") === 0) {
		        if (/^<(\w+)(\s+?.*?)*(\/>|>.*<\/\s*\1>)$/i.test(dom)) {
		            var tmpDom,
		                tblEl = /^<(tr|td)\b/i.exec(dom);
		            if (tblEl) {
		                tmpDom = document.createElement(tblEl[1]);
		            }
		            else {
		                tmpDom = document.createElement("div");
		            }
		            tmpDom.innerHTML = dom;
		            _lppEl.dom = tblEl && tmpDom || Array.prototype.slice.apply(tmpDom.childNodes);
		        }
			}
			else {
				// tagName
				_lppEl.dom = _pDom.getElementsByTagName(dom);
		    }

		}
		else if (dom.tagName) {
		    _lppEl.dom = dom;
		}
		else if (dom.tag) {
		}
		
		// 生成有效lppEl对象
		if (_lppEl.dom) {
			// 获取子元素
			_lppEl.down = function (dom) {
				var pDom = _tool.getSingleDom(_lppEl.dom);

				return new lppEl(dom, pDom);
			};

            // 追加子元素
			_lppEl.append = function (dom) {
			    var _childLppEl = dom.dom && dom || lpp(dom),
			        pDom = _tool.getSingleDom(_lppEl.dom);
			    _lppEl.children.push(_childLppEl);
			    _tool.each(_childLppEl.dom, function (dom) {
			        pDom.appendChild(dom);
			    });

			    return _childLppEl;
			};

            // 追加到某元素
			_lppEl.appendAt = function (pDom) {
			    var _pDom = pDom.dom && _tool.getSingleDom(pDom.dom) || lpp(pDom);
			    _pDom.children.push(_lppEl);
			    _tool.each(_lppEl.dom, function (dom) {
			        _pDom.dom.appendChild(dom);
			    });

			    return _lppEl;
			};

            // 插入子元素
			_lppEl.insert = function (dom, index) {
			    if (!_lppEl.children.length) {
			        return _lppEl.append(dom);
			    }

			    var _childLppEl = dom.dom && dom || lpp(dom),
		            _index = index && (index >= _lppEl.children ? _lppEl.children - 1 : index) || 0,
			        _pDom = _tool.getSingleDom(_lppEl.dom);
			    var _refDom = _tool.getSingleDom(_lppEl.children[_index].dom);
			    _lppEl.children = _lppEl.children.slice(0, _index).push(_childLppEl).concat.apply(this, _lppEl.children.slice(_index));
			    _tool.each(_childLppEl.dom, function (dom) {
			        _pDom.insertBefore(dom, _refDom);
			    });
			   

			    return _childLppEl;
			};

            // 插入到某元素
			_lppEl.insertAt = function (pDom, index) {
			    var _pDom = pDom.dom && _tool.getSingleDom(pDom.dom) || lpp(pDom);
			    if (!_pDom.children.length) {
			        return _lppEl.appendAt(_pDom);
			    }

			    var _index = index && (index >= _pDom.children ? _pDom.children - 1 : index) || 0;
			    var _refDom = _tool.getSingleDom(_pDom.children[_index].dom);
			    _pDom.children = _pDom.children.slice(0, _index).push(_lppEl).concat.apply(this, _pDom.children.slice(_index));
			    _tool.each(_lppEl.dom, function (dom) {
			        _pDom.insertBefore(dom, _refDom);
			    });

			    return _lppEl;
			};

			// 删除元素
			_lppEl.remove = function () {
			    var removedLppEl = _lppEl;
			    _tool.each(_lppEl.dom, function (dom) {
			        dom.parentNode.removeChild(dom);
			    });

				return removedLppEl;
			};

			// 获取或设置样式
			_lppEl.css = function (property, value) {
				if (arguments.length === 1) {
					return _lppEl.dom.length && _lppEl.dom[0].style[property] || _lppEl.dom.style[property];
				}
				else if (arguments.length === 2) {
					var _dom = _tool.getSingleDom(_lppEl.dom);
					_dom.style[property] = value;

					return _lppEl;
				}
				else {
					throw { name: "Arguments Exception", message: "Count of Arguments is wrong!" };
				}
			};

			_lppEl.hasClass = function (className) {
			    var reg = new RegExp('(\\s|^)' + className + '(\\s|$)'),
					_dom = _tool.getSingleDom(_lppEl.dom);
			    return el.className.match(reg);
			};

			_lppEl.addClass = function (className) {
			    var _dom = _tool.getSingleDom(_lppEl.dom);
			    if (!_lppEl.hasClass(className)) {
			        _dom.className += " " + className;
			    }
			};
			_lppEl.removeClass = function (className) {
			    if (_lppEl.hasClass(className)) {
			        var reg = new RegExp('(\\s|^)' + className + '(\\s|$)'),
						_dom = _tool.getSingleDom(_lppEl.dom);
			        _dom.className = _dom.className.replace(reg, ' ');
			    }
			};

			_lppEl.html = function (value) {
			    var _dom = _tool.getSingleDom(_lppEl.dom);
			    if (arguments.length === 0) {
			        return _dom.innerHTML;
			    }

			    _dom.innerHTML = value;
			    return _lppEl;
			};

			// 获取或设置值
			_lppEl.value = function (value) {
				var isGet = arguments.length === 0,
					_dom = _tool.getSingleDom(_lppEl.dom);

				switch (_dom.tagName.toLocaleLowerCase()) {
					case 'select':
						if (isGet) {
							return _dom.options[_dom.selectedIndex].value;
						}
						else {
							var selectedIndex = 0,
								i;
							for (i = 0; i < _el.options.length; ++i) {
								if (_el.options[i].value === value.toString()) {
									selectedIndex = i;
									break;
								}
							}
							_dom.selectedIndex = selectedIndex;
						}
						break;
					default:
						if (isGet) {
							return _dom.value;
						}
						else {
							_dom.value = value;
						}
						break;
				}

				return _lppEl;
			};

			// 获取或设置属性
			_lppEl.attr = function (property, value) {
				var _dom = _tool.getSingleDom(_lppEl.dom);
				if (arguments.length === 1) {
				    if (!lpp.util.isNothing(_dom[property])) {
				        return _dom[property];
					}
					else {
						return _dom.getAttribute(property);
					}
				}
				else if (arguments.length === 2) {
				    if (property === 'checked' && (lpp.util.getType(value) !== 'boolean' ? value == false : !value)) {
						return _lppEl;
					}
				    _dom.setAttribute(property, value);
				    return _lppEl;
				}
				else {
					throw { name: "Arguments Exception", message: "Count of Arguments is wrong!" };
				}
			};

			// 绑定事件
			_lppEl.on = function _on(event, handler) {
				var _handler = handler,
					_dom = _tool.getSingleDom(_lppEl.dom);
				if (event === "leave") {
					var handlers = _lppEl.fn.createCallback(function (handler, delay) {
						var _timer = null, _handlers = {};
						_handlers.onmouseout = function () {
							_timer = setTimeout(handler, delay);
						};

						_handlers.onmousemove = function () {
							clearTimeout(_timer);
						};

						return _handlers;
					}, handler, 350);

					_on("mouseout", handlers.onmouseout);
					_on("mousemove", handlers.onmousemove);
				}
				else {
					if (_dom.attachEvent) {
						_dom.attachEvent("on" + event, _handler);
					}
					else if (_dom.addEventListener) {
						_dom.addEventListener(event, _handler, false);
					}
				}

				return _lppEl;
			};

			// 删除绑定
			_lppEl.un = function _un(event, handler) {
				var _handler = handler,
					_dom = _tool.getSingleDom(_lppEl.dom);
				if (event === "leave") {
					var handlers = _lppEl.fn.createCallback(function (handler, delay) {
						var _timer = null, _handlers = {};
						_handlers.onmouseout = function () {
							_timer = setTimeout(handler, delay);
						};

						_handlers.onmousemove = function () {
							clearTimeout(_timer);
						};

						return _handlers;
					}, handler, 350);

					_un("mouseout", handlers.onmouseout);
					_un("mousemove", handlers.onmousemove);
				}
				else {
					if (_dom.attachEvent) {
						_dom.detachEvent("on" + event, _handler);
					}
					else if (_dom.addEventListener) {
						_dom.removeEventListener(event, _handler, false);
					}
				}

				return _lppEl;
			};

		    /*
            * @param config{Object}
            *      during{Number} unit:ms
            *      display{String} 值：hidden(使用display: none), invisible(使用visibility: hidden), transparent(仅使用opacity:0); 默认为hidden
            *      afterHide{Function}
            *          @param  lppEl{lppEl}
            */
			_lppEl.hide = function _hide(config) {
			    var _during = _tool.getConfigVal(config, 'during', 0, "number"),
                    _afterHide = _tool.getConfigVal(config, "afterHide", function () { }, "function"),
                    _display = _tool.getConfigVal(config, "display", "hidden", "string", ["hidden", "invisible", "transparent"]);
			    if (_during) {
			        _lppEl.css("opacity", "1");
			        _lppEl.css("filter", "alpha(opacity=100)");
			        var _t = _during;
			        for (var i = _during; i >= 1; --i) {
			            setTimeout(lpp.fn.createCallback(function (lppEl, t, time, isHide, afterHide, display) {
			                lppEl.css("opacity", time / t);
			                lppEl.css("filter", "alpha(opacity=" + (time / t * 100) + ")");
			                if (isHide) {
			                    if (display === "hidden") {
			                        lppEl.css("display", "none");
			                    }
			                    if (display === "invisible") {
			                        lppEl.css("visibility", "hidden");
			                    }
			                    if (display !== "transparent") {
			                        lppEl.css("opacity", 1);
			                        lppEl.css("filter", "alpha(opacity=100)");
			                    }
			                    afterHide(lppEl);
			                }
			            }, _lppEl, _t, i, i === 1, _afterHide, _display), 1 + _during - i);
			        }
			    }
			    else {
			        switch (_display){
			            case 'hidden':
			                _lppEl.css("display", "none");
			                break;
			            case 'invisible':
			                _lppEl.css("visiblity", "hidden");
			                break;
			            case 'transparent':
			                _lppEl.css("opacity", 0);
			                _lppEl.css("filter", "alpha(opacity=0)");
			                break;
			        }
			    }

			    return _lppEl;
			};

		    /*
            * @param config{Object}
            *      during{Number} unit:ms
            *      afterShow{Function}
            *          @param  lppEl{lppEl}
            */
			_lppEl.show = function (config) {
			    var _during = _tool.getConfigVal(config, 'during', 0, "number"),
                   _afterShow = _tool.getConfigVal(config, "afterShow", function () { }, "function");

			    if (_during) {
			        _lppEl.css("opacity", "0");
			        _lppEl.css("filter", "alpha(opacity=0)");
			        var _t = _during;
			        for (var i = 1; i <= _during; ++i){
			            setTimeout(lpp.fn.createCallback(function (lppEl, t, time, begin, complete, afterShow) {
			                if (begin) {
			                    lppEl.css("display", "");
			                    lppEl.css("visibility", "visible");
			                }
			                lppEl.css("opacity", time / t);
			                lppEl.css("filter", "alpha(opacity=" + (time / t * 100) + ")");
			                if (complete) {
			                    afterShow(lppEl);
			                }
			            }, _lppEl, _t, i, i === 1, i === _during, _afterShow), i);
			        }
			    }
			    else {
			        _lppEl.css("display", "");
			        _lppEl.css("visiblity", "visible");
			        _lppEl.css("opacity", 1);
			        _lppEl.css("filter", "alpha(opacity=100)");
			    }

			    return _lppEl;
			};
		}

		return _lppEl;
	}
}();