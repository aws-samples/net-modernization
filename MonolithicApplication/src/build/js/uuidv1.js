! function(e) { if ("object" == typeof exports && "undefined" != typeof module) module.exports = e();
    else if ("function" == typeof define && define.amd) define([], e);
    else {
        ("undefined" != typeof window ? window : "undefined" != typeof global ? global : "undefined" != typeof self ? self : this).uuidv1 = e() } }(function() { return function() { return function e(n, r, o) {
            function t(u, f) { if (!r[u]) { if (!n[u]) { var c = "function" == typeof require && require; if (!f && c) return c(u, !0); if (i) return i(u, !0); var s = new Error("Cannot find module '" + u + "'"); throw s.code = "MODULE_NOT_FOUND", s } var a = r[u] = { exports: {} };
                    n[u][0].call(a.exports, function(e) { return t(n[u][1][e] || e) }, a, a.exports, e, n, r, o) } return r[u].exports } for (var i = "function" == typeof require && require, u = 0; u < o.length; u++) t(o[u]); return t } }()({ 1: [function(e, n, r) { for (var o = [], t = 0; t < 256; ++t) o[t] = (t + 256).toString(16).substr(1);
            n.exports = function(e, n) { var r = n || 0,
                    t = o; return [t[e[r++]], t[e[r++]], t[e[r++]], t[e[r++]], "-", t[e[r++]], t[e[r++]], "-", t[e[r++]], t[e[r++]], "-", t[e[r++]], t[e[r++]], "-", t[e[r++]], t[e[r++]], t[e[r++]], t[e[r++]], t[e[r++]], t[e[r++]]].join("") } }, {}], 2: [function(e, n, r) { var o = "undefined" != typeof crypto && crypto.getRandomValues && crypto.getRandomValues.bind(crypto) || "undefined" != typeof msCrypto && "function" == typeof window.msCrypto.getRandomValues && msCrypto.getRandomValues.bind(msCrypto); if (o) { var t = new Uint8Array(16);
                n.exports = function() { return o(t), t } } else { var i = new Array(16);
                n.exports = function() { for (var e, n = 0; n < 16; n++) 0 == (3 & n) && (e = 4294967296 * Math.random()), i[n] = e >>> ((3 & n) << 3) & 255; return i } } }, {}], 3: [function(e, n, r) { var o, t, i = e("./lib/rng"),
                u = e("./lib/bytesToUuid"),
                f = 0,
                c = 0;
            n.exports = function(e, n, r) { var s = n && r || 0,
                    a = n || [],
                    d = (e = e || {}).node || o,
                    l = void 0 !== e.clockseq ? e.clockseq : t; if (null == d || null == l) { var p = i();
                    null == d && (d = o = [1 | p[0], p[1], p[2], p[3], p[4], p[5]]), null == l && (l = t = 16383 & (p[6] << 8 | p[7])) } var v = void 0 !== e.msecs ? e.msecs : (new Date).getTime(),
                    y = void 0 !== e.nsecs ? e.nsecs : c + 1,
                    m = v - f + (y - c) / 1e4; if (m < 0 && void 0 === e.clockseq && (l = l + 1 & 16383), (m < 0 || v > f) && void 0 === e.nsecs && (y = 0), y >= 1e4) throw new Error("uuid.v1(): Can't create more than 10M uuids/sec");
                f = v, c = y, t = l; var w = (1e4 * (268435455 & (v += 122192928e5)) + y) % 4294967296;
                a[s++] = w >>> 24 & 255, a[s++] = w >>> 16 & 255, a[s++] = w >>> 8 & 255, a[s++] = 255 & w; var b = v / 4294967296 * 1e4 & 268435455;
                a[s++] = b >>> 8 & 255, a[s++] = 255 & b, a[s++] = b >>> 24 & 15 | 16, a[s++] = b >>> 16 & 255, a[s++] = l >>> 8 | 128, a[s++] = 255 & l; for (var x = 0; x < 6; ++x) a[s + x] = d[x]; return n || u(a) } }, { "./lib/bytesToUuid": 1, "./lib/rng": 2 }] }, {}, [3])(3) });