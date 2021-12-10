﻿// (c) 2019 Max Feingold

using System.Collections.Generic;

namespace System.Web.UI
{
    public enum HtmlTextWriterTag
    {
        Unknown = 0,
        A = 1,
        Acronym = 2,
        Address = 3,
        Area = 4,
        B = 5,
        Base = 6,
        Basefont = 7,
        Bdo = 8,
        Bgsound = 9,
        Big = 10,
        Blockquote = 11,
        Body = 12,
        Br = 13,
        Button = 14,
        Caption = 15,
        Center = 16,
        Cite = 17,
        Code = 18,
        Col = 19,
        Colgroup = 20,
        Dd = 21,
        Del = 22,
        Dfn = 23,
        Dir = 24,
        Div = 25,
        Dl = 26,
        Dt = 27,
        Em = 28,
        Embed = 29,
        Fieldset = 30,
        Font = 31,
        Form = 32,
        Frame = 33,
        Frameset = 34,
        H1 = 35,
        H2 = 36,
        H3 = 37,
        H4 = 38,
        H5 = 39,
        H6 = 40,
        Head = 41,
        Hr = 42,
        Html = 43,
        I = 44,
        Iframe = 45,
        Img = 46,
        Input = 47,
        Ins = 48,
        Isindex = 49,
        Kbd = 50,
        Label = 51,
        Legend = 52,
        Li = 53,
        Link = 54,
        Map = 55,
        Marquee = 56,
        Menu = 57,
        Meta = 58,
        Nobr = 59,
        Noframes = 60,
        Noscript = 61,
        Object = 62,
        Ol = 63,
        Option = 64,
        P = 65,
        Param = 66,
        Pre = 67,
        Q = 68,
        Rt = 69,
        Ruby = 70,
        S = 71,
        Samp = 72,
        Script = 73,
        Select = 74,
        Small = 75,
        Span = 76,
        Strike = 77,
        Strong = 78,
        Style = 79,
        Sub = 80,
        Sup = 81,
        Table = 82,
        Tbody = 83,
        Td = 84,
        Textarea = 85,
        Tfoot = 86,
        Th = 87,
        Thead = 88,
        Title = 89,
        Tr = 90,
        Tt = 91,
        U = 92,
        Ul = 93,
        Var = 94,
        Wbr = 95,
        Xml = 96
    }

    public static class HtmlTextWriterTagExtensions
    {
        static Dictionary<HtmlTextWriterTag, string> s_attributes = new Dictionary<HtmlTextWriterTag, string>
        {
            { HtmlTextWriterTag.A, "a" },
            { HtmlTextWriterTag.Acronym, "acronym" },
            { HtmlTextWriterTag.Address, "address" },
            { HtmlTextWriterTag.Area, "area" },
            { HtmlTextWriterTag.B, "b" },
            { HtmlTextWriterTag.Base, "base" },
            { HtmlTextWriterTag.Basefont, "basefont" },
            { HtmlTextWriterTag.Bdo, "bdo" },
            { HtmlTextWriterTag.Bgsound, "bgsound" },
            { HtmlTextWriterTag.Big, "big" },
            { HtmlTextWriterTag.Blockquote, "blockquote" },
            { HtmlTextWriterTag.Body, "body" },
            { HtmlTextWriterTag.Br, "br" },
            { HtmlTextWriterTag.Button, "button" },
            { HtmlTextWriterTag.Caption, "caption" },
            { HtmlTextWriterTag.Center, "center" },
            { HtmlTextWriterTag.Cite, "cite" },
            { HtmlTextWriterTag.Code, "code" },
            { HtmlTextWriterTag.Col, "col" },
            { HtmlTextWriterTag.Colgroup, "colgroup" },
            { HtmlTextWriterTag.Dd, "dd" },
            { HtmlTextWriterTag.Del, "del" },
            { HtmlTextWriterTag.Dfn, "dfn" },
            { HtmlTextWriterTag.Dir, "dir" },
            { HtmlTextWriterTag.Div, "div" },
            { HtmlTextWriterTag.Dl, "dl" },
            { HtmlTextWriterTag.Dt, "dt" },
            { HtmlTextWriterTag.Em, "em" },
            { HtmlTextWriterTag.Embed, "embed" },
            { HtmlTextWriterTag.Fieldset, "fieldset" },
            { HtmlTextWriterTag.Font, "font" },
            { HtmlTextWriterTag.Form, "form" },
            { HtmlTextWriterTag.Frame, "frame" },
            { HtmlTextWriterTag.Frameset, "frameset" },
            { HtmlTextWriterTag.H1, "h1" },
            { HtmlTextWriterTag.H2, "h2" },
            { HtmlTextWriterTag.H3, "h3" },
            { HtmlTextWriterTag.H4, "h4" },
            { HtmlTextWriterTag.H5, "h5" },
            { HtmlTextWriterTag.H6, "h6" },
            { HtmlTextWriterTag.Head, "head" },
            { HtmlTextWriterTag.Hr, "hr" },
            { HtmlTextWriterTag.Html, "html" },
            { HtmlTextWriterTag.I, "i" },
            { HtmlTextWriterTag.Iframe, "iframe" },
            { HtmlTextWriterTag.Img, "img" },
            { HtmlTextWriterTag.Input, "input" },
            { HtmlTextWriterTag.Ins, "ins" },
            { HtmlTextWriterTag.Isindex, "isindex" },
            { HtmlTextWriterTag.Kbd, "kbd" },
            { HtmlTextWriterTag.Label, "label" },
            { HtmlTextWriterTag.Legend, "legend" },
            { HtmlTextWriterTag.Li, "li" },
            { HtmlTextWriterTag.Link, "link" },
            { HtmlTextWriterTag.Map, "map" },
            { HtmlTextWriterTag.Marquee, "marquee" },
            { HtmlTextWriterTag.Menu, "menu" },
            { HtmlTextWriterTag.Meta, "meta" },
            { HtmlTextWriterTag.Nobr, "nobr" },
            { HtmlTextWriterTag.Noframes, "noframes" },
            { HtmlTextWriterTag.Noscript, "noscript" },
            { HtmlTextWriterTag.Object, "object" },
            { HtmlTextWriterTag.Ol, "ol" },
            { HtmlTextWriterTag.Option, "option" },
            { HtmlTextWriterTag.P, "p" },
            { HtmlTextWriterTag.Param, "param" },
            { HtmlTextWriterTag.Pre, "pre" },
            { HtmlTextWriterTag.Q, "q" },
            { HtmlTextWriterTag.Rt, "rt" },
            { HtmlTextWriterTag.Ruby, "ruby" },
            { HtmlTextWriterTag.S, "s" },
            { HtmlTextWriterTag.Samp, "samp" },
            { HtmlTextWriterTag.Script, "script" },
            { HtmlTextWriterTag.Select, "select" },
            { HtmlTextWriterTag.Small, "small" },
            { HtmlTextWriterTag.Span, "span" },
            { HtmlTextWriterTag.Strike, "strike" },
            { HtmlTextWriterTag.Strong, "strong" },
            { HtmlTextWriterTag.Style, "style" },
            { HtmlTextWriterTag.Sub, "sub" },
            { HtmlTextWriterTag.Sup, "sup" },
            { HtmlTextWriterTag.Table, "table" },
            { HtmlTextWriterTag.Tbody, "tbody" },
            { HtmlTextWriterTag.Td, "td" },
            { HtmlTextWriterTag.Textarea, "textarea" },
            { HtmlTextWriterTag.Tfoot, "tfoot" },
            { HtmlTextWriterTag.Th, "th" },
            { HtmlTextWriterTag.Thead, "thead" },
            { HtmlTextWriterTag.Title, "title" },
            { HtmlTextWriterTag.Tr, "tr" },
            { HtmlTextWriterTag.Tt, "tt" },
            { HtmlTextWriterTag.U, "u" },
            { HtmlTextWriterTag.Ul, "ul" },
            { HtmlTextWriterTag.Var, "var" },
            { HtmlTextWriterTag.Wbr, "wbr" },
            { HtmlTextWriterTag.Xml, "xml" },
        };

        public static string ToName(this HtmlTextWriterTag attribute) => s_attributes[attribute];
    }
}
