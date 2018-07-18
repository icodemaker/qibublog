!function() {

function Captcha(wrapper) {
    var t = this, img = t._img = $("img.captcha-img", wrapper);

    t.bgColor = img.attr("data-bgcolor");
    t.formName = img.attr("data-formname");
    t.src = img.attr("data-src");

    $("input[name=captcha]", wrapper).attr("autocomplete", "off");

    img.click(function () {
        t.refresh();
    }).click();
}
$.extend(Captcha.prototype, {
    refresh : function() {
        var t = this;
        t.rnd = parseInt( Math.random() * 100000 );
        t._img.prop( "src", (t.src + "?bgColor={$bgColor}&formName={$formName}&rnd={$rnd}").replace(/\{\$(\w+)\}/g, function($0, $1) {
            return t[$1] || "";
        }) );
    }
});


window.Captcha = Captcha;

}();