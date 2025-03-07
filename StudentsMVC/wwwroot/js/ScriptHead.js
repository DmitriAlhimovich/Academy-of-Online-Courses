﻿function draw() {
    con.clearRect(0, 0, WIDTH, HEIGHT);
    for (var e = 0; e < pxs.length; e++) {
        pxs[e].fade();
        pxs[e].move();
        pxs[e].draw()
    }
}

function Circle() {
    var scrollHeight = Math.max(
        document.body.scrollHeight, document.documentElement.scrollHeight,
        document.body.offsetHeight, document.documentElement.offsetHeight,
        document.body.clientHeight, document.documentElement.clientHeight
    );
    var scrollWidth = Math.max(
        document.body.scrollWidth, document.documentElement.scrollWidth,
        document.body.offsetWidth, document.documentElement.offsetWidth,
        document.body.clientWidth, document.documentElement.clientWidth
    );
    WIDTH = scrollWidth;
    HEIGHT = scrollHeight;
    this.s = {
        ttl: 8e3,
        xmax: 5,
        ymax: 2,
        rmax: 10,
        rt: 1,
        xdef: 960,
        ydef: 540,
        xdrift: 4,
        ydrift: 4,
        random: true,
        blink: true
    };
    this.reset = function () {
        this.x = this.s.random ? WIDTH * Math.random() : this.s.xdef;
        this.y = this.s.random ? HEIGHT * Math.random() : this.s.ydef;
        this.r = (this.s.rmax - 1) * Math.random() + 1;
        this.dx = Math.random() * this.s.xmax * (Math.random() < .5 ? -1 : 1);
        this.dy = Math.random() * this.s.ymax * (Math.random() < .5 ? -1 : 1);
        this.hl = this.s.ttl / rint * (this.r / this.s.rmax);
        this.rt = Math.random() * this.hl;
        this.s.rt = Math.random() + 1;
        this.stop = Math.random() * .2 + .4;
        this.s.xdrift *= Math.random() * (Math.random() < .5 ? -1 : 1);
        this.s.ydrift *= Math.random() * (Math.random() < .5 ? -1 : 1)
    };
    this.fade = function () {
        this.rt += this.s.rt
    };
    this.draw = function () {
        if (this.s.blink && (this.rt <= 0 || this.rt >= this.hl)) this.s.rt = this.s.rt * -1;
        else if (this.rt >= this.hl) this.reset();
        var e = 1 - this.rt / this.hl;
        con.beginPath();
        con.arc(this.x, this.y, this.r, 0, Math.PI * 2, true);
        con.closePath();
        var t = this.r * e;
        g = con.createRadialGradient(this.x, this.y, 0, this.x, this.y, t <= 0 ? 1 : t);
        g.addColorStop(0, "rgba(255,255,255," + e + ")");
        g.addColorStop(this.stop, "rgba(77,101,181," + e * .6 + ")");
        g.addColorStop(1, "rgba(77,101,181,0)");
        con.fillStyle = g;
        con.fill()
    };
    this.move = function () {
        var scrollHeight = Math.max(
            document.body.scrollHeight, document.documentElement.scrollHeight,
            document.body.offsetHeight, document.documentElement.offsetHeight,
            document.body.clientHeight, document.documentElement.clientHeight
        );
        var scrollWidth = Math.max(
            document.body.scrollWidth, document.documentElement.scrollWidth,
            document.body.offsetWidth, document.documentElement.offsetWidth,
            document.body.clientWidth, document.documentElement.clientWidth
        );
        WIDTH = scrollWidth;
        HEIGHT = scrollHeight;
        this.x += this.rt / this.hl * this.dx;
        this.y += this.rt / this.hl * this.dy;
        if (this.x > WIDTH || this.x < 0) this.dx *= -1;
        if (this.y > HEIGHT || this.y < 0) this.dy *= -1
    };
    this.getX = function () {
        return this.x
    };
    this.getY = function () {
        return this.y
    }
}
var WIDTH;
var HEIGHT;
var canvas;
var con;
var g;
var pxs = new Array;
var rint = 60;
$(document).ready(function () {
    var scrollHeight = Math.max(
        document.body.scrollHeight, document.documentElement.scrollHeight,
        document.body.offsetHeight, document.documentElement.offsetHeight,
        document.body.clientHeight, document.documentElement.clientHeight
    );
    var scrollWidth = Math.max(
        document.body.scrollWidth, document.documentElement.scrollWidth,
        document.body.offsetWidth, document.documentElement.offsetWidth,
        document.body.clientWidth, document.documentElement.clientWidth
    );
    $("#container").width(WIDTH).height(HEIGHT);
    WIDTH = scrollWidth;
    HEIGHT = scrollHeight;
    canvas = document.getElementById("pixie");
    $(canvas).attr("width", WIDTH).attr("height", HEIGHT);
    con = canvas.getContext("2d");
    for (var e = 0; e < 100; e++) {
        pxs[e] = new Circle;
        pxs[e].reset()
    }
    setInterval(draw, rint)
});
$(".services .header2 .service-header").hover(function () {
    var e = $(this);
    e.find("h3").hide();
    $(this).parent().find(".header-bg").stop(true, true).animate({
        width: "100%"
    }, "fast", function () {
        e.find("h3").addClass("active").fadeIn("fast")
    })
}, function () {
    var e = $(this);
    e.find("h3").hide();
    e.parent().find(".header-bg").stop(true, true).animate({
        width: 70
    }, "fast", function () {
        e.find("h3").removeClass("active").fadeIn("fast")
    })
})