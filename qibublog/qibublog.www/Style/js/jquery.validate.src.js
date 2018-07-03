/*!
 * Form Validation Plugin v1.0.0 For jQuery 1.3+
 * http://heeroluo.net/
 *
 * Copyright 2011, Heero.Luo
 * Dual licensed under the MIT or GPL Version 2 licenses.
 *
 * Date: Thu Apr 26 2012 16:10:28 GMT+0800
 */
(function ($, undefined) { "use strict";

/*
* 验证当前表单元素
* @param {Object} 参数
*/
$.fn.validate = function (options) {
    return this.each(function (i, el) {
        new $.validate( $(el), options );
    });
};


/*
* 表单验证
* @param {HTMLElement} 表单元素
* @param {Object} 参数
*/
$.validate = function (el, options) {
    var self = this, o = $.extend({ }, $.validate.defaults, options);
	self.options = o;
	self._el = el;
	// self._extraErrors = [ ];
	
    if (!o.flow || !el.length) { return; }
	
	self._explainFlow();
	
	if (o.enableBlurChecking) { self._bindCheckingWhenBlur(); }
	
	self._bindCheckingWhenSubmit();
};


$.extend($.validate.prototype, {
	// 解析验证流程
    _explainFlow : function() {
		var flow = this.options.flow;
		$.each(flow, function (i, f) {
			var rules = f.rules, ruleType = typeof rules;
			if (!rules) { return; }

			// 规则之间通过分号隔开
			if ("string" === ruleType) { rules = rules.split(/\s*;\s*/); }

			if (ruleType != "function") {
				$.each(rules, function (j, r) {
					if ("string" === typeof r) {
						// 规则名和规则参数通过冒号隔开，多个规则参数通过逗号隔开
						r = r.split(/\s*[:,]\s*/);

						var fn = $.validate.rules[ r[0] ];
						// 找到规则函数后，规则名已经没用了，清掉
						r[0] = undefined;

						// 把规则“编译”成函数
						rules[j] = (function (fn, args) {
							return function (val) {
								args = args.slice(); // 避免覆盖了原数组，复制一个
								args[0] = val;
								return fn.apply(this, args);
							}
						})(fn, r);
					}
				});

				flow[i].rules = rules;
			} else {
				flow[i].rules = [rules];
			}
		});
	},
	
	// 触发字段错误事件
	fieldCorrect : function(field) {
		var self = this, o = self.options, extraErrors = self._extraErrors, pos;
		/*for (var i = extraErrors.length - 1; i >= 0; i--) {
			if ( extraErrors[i].field.get(0) === field.get(0) ) {
				pos = i;
				break;
			}
		}
		if (pos != null) {
			extraErrors.splice(pos, 1);
		}*/
		o.onfieldcorrect && o.onfieldcorrect.call(self, field);
	},
	
	// 触发字段正确事件
	fieldError : function(field, errorText, isExtra) {
		var self = this, o = self.options;
		/*if (isExtra) {
			self._extraErrors.push({
				errorText : errorText,
				field : field
			});
		}*/
		o.onfielderror && o.onfielderror.call(self, field, errorText);
	},
	
	// 检查单个字段
	_checkSingleField : function(f, val, field) {
		var errorText, self = this;
		
		// 是否必填字段
		if (false !== f.required && !val) {
			errorText = f.errorText;
		}
		
		if (!errorText && val && f.rules) {
			$.each(f.rules, function (j, r) {
				if ( false === r.call(self, val, field) ) {
					errorText = f.errorText;
					return false;
				}
			});
		}
		
		return errorText;
	},
	
	// 控件失去焦点时的验证
	_bindCheckingWhenBlur : function() {
		var self = this, o = self.options;
		
		$("[name][type!=hidden][type!=button][type!=submit]", self._el).blur(function() {
			if ( this.getAttribute("data-ignorecheck") ) { return; }
		
			var val = $.trim(this.value), field = $(this),
				flow = self.options.flow, errorText, hasAsyncChecking, required;
			$.each(flow, function(i, f) {
				f = flow[i];
				if (f.fields === field.get(0).name) {
					if (f.isAsync) { hasAsyncChecking = true; }
					required = f.required;
					errorText = self._checkSingleField(f, val, field);
					if (errorText) { return false; }
				}
			});
			
			if (errorText) {
				self.fieldError(field, errorText);
			} else if ( !hasAsyncChecking && (required !== false || val) ) {
				self.fieldCorrect(field);
			}
		});
	},

	// 获取表单各字段的值
	val : function() {
		var data = this._el.serializeArray(), trueData = { };

		// 把数据数组解释为字典
		$.each(data, function (i, d) {
			var temp = d.name;
			trueData[temp] = trueData[temp] || [];
			trueData[temp].push( $.trim(d.value) );
		});
		$.each(trueData, function (i, d) {
			if (d != null) {
				if (d.length > 1) {
					if (o.multiValueSeparator != null) {
						trueData[i] = d.join(", ");
					}
				} else {
					trueData[i] = d[0];
				}
			}
		});
		
		return trueData;
	},
	
	// 提交表单时验证
	_bindCheckingWhenSubmit : function() {
		var self = this, o = self.options;
		
		self._el.bind("submit", function (e) {
			var form = this;
			
			if (o.beforeformvalidate) {
				o.beforeformvalidate.call(self, form);
			}

			var trueData = self.val(), errorTexts = [ ], first;

			$.each(o.flow, function (i, f) {
				// 表单提交时不进行异步检测
				// if (f.isAsync) { return; }
				
				var fieldNames, errorText, fields;
				
				if (f.fields) {
					fieldNames = f.fields.split(/\s*,\s*/);
					fields = $("[name=" + fieldNames.join("], [name=") + "]", form);
				}
				
				if (!fieldNames || fieldNames.length > 1) {	// 当涉及验证的字段多于1个时，仅支持单个自定义函数验证
					var args = [ ];
					// 获取相关字段的值
					fieldNames && $.each(fieldNames, function (j, n) {
						args[j] = trueData[n];
					});
					
					args.push(fields);
					
					if ( false === f.rules[0].apply(self, args) ) {
						errorText = f.errorText;
					}
				} else {
					errorText = self._checkSingleField(f, trueData[f.fields], fields);
				}
				
				if (errorText) {
					errorTexts.push(errorText);
					if (!first && fields) { first = $( fields.get(0) ); }
				}
			});
			
			/*$.each(self._extraErrors, function(i, e) {
				errorTexts.push(e.errorText);
				if (!first) {
					first = e.field;
				}
			});*/
			
			var hasError = errorTexts.length > 0;
			
			if (o.afterformvalidate) {
				o.afterformvalidate.call(self, form, hasError);
			}

			if (hasError) {
				e.preventDefault();
				o.onformerror && o.onformerror.call(self, errorTexts, first);
			} else {
                o.submit && o.submit.call(self, form, e, trueData);
            }
		});
	}
});


/*
* 默认参数
*/
$.validate.defaults = {
	enableBlurChecking : false,
	multiValueSeparator : ", ",
    onformerror: function (texts, first) {
		alert( texts.join("\r\n") );
		first && first.focus();
	}
};

/*
* 验证规则
*/
$.validate.rules = {
    /*
    * 最小值验证
    */
    min: function (val, ref) { return Number(val) >= ref; },

    /*
    * 最大值验证
    */
    max: function (val, ref) { return Number(val) <= ref; },

    /*
    * 最小值验证
    */
    minlength: function (val, ref) { return val.length >= ref; },

    /*
    * 最大值验证
    */
    maxlength: function (val, ref) { return val.length <= ref; },

    /*
    * 枚举验证
    */
    oneof: function (val) {
        for (var i = arguments.length; i >= 1; i--) {
            if (arguments[i] !== val) {
                return false;
            }
        }
        return true;
    },

    /*
    * Email验证规则
    */
    isemail: function (val) {
        return /^([.a-zA-Z0-9_-])+@([a-zA-Z0-9_-])+((\.[a-zA-Z0-9_-]+)*)\.[a-zA-Z]+$/.test(val);
    }
};

})(jQuery);