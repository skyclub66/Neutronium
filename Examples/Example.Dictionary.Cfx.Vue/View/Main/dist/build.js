!function(t){function e(c){if(n[c])return n[c].exports;var r=n[c]={i:c,l:!1,exports:{}};return t[c].call(r.exports,r,r.exports,e),r.l=!0,r.exports}var n={};e.m=t,e.c=n,e.i=function(t){return t},e.d=function(t,n,c){e.o(t,n)||Object.defineProperty(t,n,{configurable:!1,enumerable:!0,get:c})},e.n=function(t){var n=t&&t.__esModule?function(){return t.default}:function(){return t};return e.d(n,"a",n),n},e.o=function(t,e){return Object.prototype.hasOwnProperty.call(t,e)},e.p="",e(e.s=6)}([function(t,e){t.exports=function(t,e,n,c){var r,o=t=t||{},a=typeof t.default;"object"!==a&&"function"!==a||(r=t,o=t.default);var u="function"==typeof o?o.options:o;if(e&&(u.render=e.render,u.staticRenderFns=e.staticRenderFns),n&&(u._scopeId=n),c){var i=u.computed||(u.computed={});Object.keys(c).forEach(function(t){var e=c[t];i[t]=function(){return e}})}return{esModule:r,exports:o,options:u}}},function(t,e,n){n(11);var c=n(0)(n(3),n(18),null,null);t.exports=c.exports},function(t,e){t.exports=Vue},function(t,e,n){"use strict";Object.defineProperty(e,"__esModule",{value:!0});var c=n(7),r=(n.n(c),n(8)),o=(n.n(r),n(14)),a=n.n(o),u=n(15),i=n.n(u),s={viewModel:Object};e.default={name:"app",props:s,components:{rawDisplay:a.a,commandButton:i.a},data:function(){return this.viewModel}}},function(t,e,n){"use strict";Object.defineProperty(e,"__esModule",{value:!0}),e.default={props:{object:Object}}},function(t,e,n){"use strict";Object.defineProperty(e,"__esModule",{value:!0});var c=n(12);e.default={mixins:[c.a],props:{name:String}}},function(t,e,n){"use strict";Object.defineProperty(e,"__esModule",{value:!0});var c=n(2),r=n.n(c),o=n(1),a=n.n(o);r.a.component("app",a.a)},function(t,e){},function(t,e){},function(t,e){},function(t,e){},function(t,e){},function(t,e,n){"use strict";const c={props:{command:{required:!0,validator:function(t){return"object"==typeof t}},arg:{type:Object,required:!1,default:null}},computed:{canExecute:function(){return null!==this.command&&(!this.command.hasOwnProperty("CanExecuteValue")||this.command.CanExecuteValue)}},watch:{"command.CanExecuteCount":function(){this.computeCanExecute()},arg:function(){this.computeCanExecute()}},methods:{computeCanExecute:function(){null!==this.command&&this.command.CanExecute&&this.command.CanExecute(this.arg)},execute:function(){if(this.canExecute){const t=this.arg,e={arg:t,cancel:!1};this.$emit("beforeExecute",e),e.cancel||(this.command.Execute(t),this.$emit("afterExecute",t))}}}};e.a=c},function(t,e,n){t.exports=n.p+"810a1b930979de83478be0f458b51dd8.png"},function(t,e,n){n(9);var c=n(0)(n(4),n(16),null,null);t.exports=c.exports},function(t,e,n){n(10);var c=n(0)(n(5),n(17),null,null);t.exports=c.exports},function(t,e){t.exports={render:function(){var t=this,e=t.$createElement;return(t._self._c||e)("pre",[t._v(t._s(t.object))])},staticRenderFns:[]}},function(t,e){t.exports={render:function(){var t=this,e=t.$createElement;return(t._self._c||e)("button",{staticClass:"btn btn-default",attrs:{type:"button"},on:{click:t.execute}},[t._v(t._s(t.name))])},staticRenderFns:[]}},function(t,e,n){t.exports={render:function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{staticClass:"fluid container",attrs:{id:"app"}},[t._m(0),t._v(" "),n("div",{staticClass:"list-group col-md-6"},[n("h3",[t._v("ExpandoObject")]),t._v(" "),n("raw-display",{attrs:{object:t.ExpandoObject}}),t._v(" "),n("command-button",{staticClass:"btn-primary",attrs:{command:t.ChangeAttribute,name:"Change attribute"}}),t._v(" "),n("command-button",{staticClass:"btn-secondary",attrs:{command:t.AddAttribute,name:"Add attribute"}})],1),t._v(" "),n("div",{staticClass:"list-group col-md-6"},[n("h3",[t._v("DynamicObject")]),t._v(" "),n("raw-display",{attrs:{object:t.DynamicObject}})],1)])},staticRenderFns:[function(){var t=this,e=t.$createElement,c=t._self._c||e;return c("div",{staticClass:"jumbotron logo",attrs:{id:"main-menu"}},[c("img",{attrs:{src:n(13)}}),t._v(" "),c("p",[t._v("Neutronium ExpandoObject and DynamicObject mapping")])])}]}}]);