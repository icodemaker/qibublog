!function($) { "use strict";

// HTTP工具类
var httpUtility = window.httpUtility = {
    _htmlEntities : {
        "<" : "&lt;",
        ">" : "&gt;",
        '"' : "&quot;",
        "'" : "'&#039;"
    },
    
    // 编码HTML特殊字符
    htmlEncode : function(str) {
        var t = this;
        return str ? str.replace(/[<>"']/g, function($0) {
            return t._htmlEntities[$0];
        }) : str;
    }
};

// 日期工具
var dateUtility = window.dateUtility = {

    _cSharpJSONDate : /^\/Date\((\d+)\)\/$/,

    parseCSharpJSONDate : function(val) {
        return this._cSharpJSONDate.test(val) ? new Date(parseInt(RegExp.$1)) : null;
    },

    format : function(t, format) {
        var toTwoDigit = function(val) { return ("0" + val).slice(-2); },
			to12Hours = function(hours) { return hours > 12 ? hours - 12 : hours; };
			
		return format ? format.replace(/y+|m+|d+|h+|s+|H+|M+/g, function($0) {
			switch ($0) {
				case 'yyyy': return t.getFullYear();
				case 'yy': return toTwoDigit(t.getFullYear());
				case 'MM': return toTwoDigit(t.getMonth() + 1);
				case 'M': return t.getMonth() + 1;
				case 'dd': return toTwoDigit(t.getDate());
				case 'd': return t.getDate();
				case 'HH': return toTwoDigit(t.getHours());
				case 'H': return t.getHours();
				case 'hh': return toTwoDigit(to12Hours(t.getHours()));
				case 'h': return to12Hours( t.getHours() );
				case 'mm': return toTwoDigit(t.getMinutes());
				case 'm': return t.getMinutes();
				case 'ss': return toTwoDigit(t.getSeconds());
				case 's': return t.getSeconds();
				default: return $0;
			}
		}) : Date.prototype.toString.call(t);
    },

    iFormat : function(date) {
        var now = new Date(), diff = now - date;
        if (diff < 60000) {
            return Math.max(1, parseInt(diff / 1000)) + "秒前";
        } else if (diff && diff < 3600000) {
            return Math.max(1, parseInt(diff / 60000)) + "分钟前";
        } else if (
            date.getFullYear() === now.getFullYear() &&
            date.getMonth() === now.getMonth() &&
            date.getDate()===now.getDate()
        ) {
            return this.format(date, "今天 HH:mm");
        } else {
            return this.format(date, "yyyy年M月d日 H:mm");
        }
    }
};


/*var header = $("#header"), footer = $("#footer");

// 设定主体区域的最小高度，使页脚在最底
function setBodyHeight() {
    $("body > .body").css("min-height",  document.documentElement.clientHeight
        - header.outerHeight(true)
        - footer.outerHeight(true)
    );
}
setBodyHeight();
$(window).on("resize", setBodyHeight);*/


// 快捷导航
var currentPosition = $("#current-position").click(function() {
    $("#express-nav").css("visibility", "visible").animate({
        opacity : 1
    });
    $(this).addClass("expanded");
});
$("#footer nav").mouseleave(function() {
    $("#express-nav").animate({
        opacity : 0
    }, function() {
        $(this).css("visibility", "");
    });
    currentPosition.removeClass("expanded");
});

var commentManage = $("#comment-manage");
if (commentManage.length) {
    var getUnreviewedCount = function() {
        $.ajax({
            url : "/Manage_Comment/UnreviewedCount",
            success : function(data) {
                data = parseInt(data);
                if (data && data > 0) {
                    $("em", commentManage).text(data);
                    commentManage.css("display", "inline");
                } else {
                    commentManage.css("display", "none");
                }

                setTimeout(getUnreviewedCount, 120000);
            }
        });
    }
    getUnreviewedCount();
}


// 返回页顶
$("#to-top").click(function(e) {
    e.preventDefault();
    window.scrollTo(0, 0);
});


// 输入框focus和blur效果
$("p.textbox-with-label .form-text, div.textbox-with-label .form-text, span.textbox-with-label .form-text")
    .focus(function () {
        $(this).parent().addClass("textbox-with-label-focus");
	    $("+ label", this).css("display", "none");
    }).blur(function () {
        $(this).parent().removeClass("textbox-with-label-focus");
	    $("+ label", this).css("display", $.trim(this.value) ? "none" : "block");
    }).blur();

}(jQuery);