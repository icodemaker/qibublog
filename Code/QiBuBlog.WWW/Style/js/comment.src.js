!function($) { "use strict";

// 评论读取器
var commentReader = {
    // JSON地址
    url : "/Comment/List",
    // 评论列表容器
    wrapper : $("#comment-list > div"),
    // 文章编号
    articleId : $("#comment-articleid").val(),

    // 评论列表模版
    listTemplate : '\<# for (var i = 0; i < data.length; i++) {\ #>\
<article<# if (data[i].AuthorId > 0) { #> class="comment-by-blogger"<# } #>>\
    <header>\
        <address class="comment-author">\
    <# if (data[i].HomePage) { #>\
        <a href="<#=data[i].HomePage #>" target="_blank"><#=data[i].AuthorName #></a>\
    <# } else { #>\
        <#=data[i].AuthorName #>\
    <# } #></address> 在 <time><#=dateUtility.iFormat(dateUtility.parseCSharpJSONDate(data[i].PostTime)) #></time> 说：\
    </header>\
    <div class="comment-content"><#=httpUtility.htmlEncode(data[i].Content).replace(/\\r\\n/g, "<br />") #></div>\
</article><# } #>\
<div class="pager"></div>',

    // 提示信息模版
    tipsTemplate : '<p class="comment-tips"><#=tips#></p>',

    // 初始化
    init : function() { if (this.articleId) { this.load(1); } },

    // 读取评论
    load : function(page, isResetPosition) {
        var t = this;
        this.wrapper.empty().html($.ui.parseTpl(this.tipsTemplate, {
            tips : "正在读取评论，请稍候..."
        }));

        $.ajax({
            url : this.url,
            dataType : "json",
            data : {
                articleId : this.articleId,
                page : page
            },
            success : function(ret) { t.render(ret, isResetPosition); }
        });
    },

    // 渲染评论
    render : function(ret, isResetPosition) {
        if (ret.Data && ret.Data.length) {
            this.wrapper.html(
                $.ui.parseTpl(this.listTemplate, {
                    data : ret.Data
                })
            );

            $("div.pager", this.wrapper).pager({
                pageCount : ret.PageCount,
                currentPage : ret.CurrentPage,
                hrefTpl : "#comment-<#=page#>",
                onclick : function(e) {
                    if ( /comment-(\d+)$/.test(this.href) ) {
                        e.preventDefault();
                        commentReader.load(RegExp.$1, true);
                    }
                }
            });
        } else {
            this.wrapper.html(
                $.ui.parseTpl(this.tipsTemplate, {
                    tips : "暂无评论"
                })
            );
        }

        if (isResetPosition) {
            window.location.hash = "comment-list";
        }
    }
};

commentReader.init();


var commentCaptcha = new Captcha($("div.captcha")),
    minCommentLen = parseInt( $("#comment-minlength").val() ),
    maxCommentLen = parseInt( $("#comment-maxlength").val() );

var validateFlow = [
    {
        fields: "Content",
        errorText: "评论内容不能少于" + minCommentLen + "个字",
        rules : function(val) { return val.length >= minCommentLen; }
    },
    {
        fields: "Content",
        errorText: "评论内容不能多于" + maxCommentLen + "个字",
        required : false,
        rules : function(val) { return val.length <= maxCommentLen; }
    }
];
if (!$("p.comment-user").length) {
    validateFlow.push(
        {
            fields : "Email",
            rules : "isemail",
            errorText : "Email格式错误",
            required : false
        },
        { fields: "AuthorName", errorText: "请填写您的称呼" },
        { fields: "captcha",  errorText: "请填写验证码" }
    );
}

$("div.comment-form form").validate({
    flow: validateFlow,

    submit : function(form, e, data) {
        e.preventDefault();

        var btn = $("input[type=submit]", form);
        if (btn.attr("disabled")) {
            alert("正在提交评论，请稍后");
            return;
        }
        
        btn.val("发表中，请稍后").attr("disabled", true);

        $.ajax({
            url : form.action,
            type : "POST",
            dataType : "json",
            data : data,
            success : function(ret) {
                if (ret.Message) { alert(ret.Message); }
                
                if (ret.hasOwnProperty("CommentData")) {
                    if (ret.CommentData != null) {
                        commentReader.render(ret.CommentData, true);
                    }
                    $(form).trigger("reset");
                }
                
                commentCaptcha.refresh();
            },
            complete : function() { btn.val("发 表").attr("disabled", false); }
        });
    }
}).bind("reset", function() {
    var t = this;
    setTimeout(function() {
        $("[name]", t).blur();
    }, 10);
});

}(jQuery);