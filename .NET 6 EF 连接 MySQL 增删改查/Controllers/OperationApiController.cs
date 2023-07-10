using Microsoft.AspNetCore.Mvc;
using Model.ViewModel;
using Model;
using System.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OperationApiController : ControllerBase
    {

        private readonly IUserInfoService _userInfoService;

        public OperationApiController(IUserInfoService userInfoService)
        {
            _userInfoService = userInfoService;
        }

       
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="page">当前页码</param>
        /// <param name="limit">显示条数</param>
        /// <param name="userName">用户姓名</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<WebApiCallBack> GetUserInfo(int page = 1, int limit = 15, string userName = "")
        {
            var result = await _userInfoService.GetPageListData(page, limit, userName);
            var jm = new WebApiCallBack();
            if (result.Code == 200)
            {
                jm.code = 0;
                jm.count = result.TotalCount;
                jm.data = result.DataList;
            }
            else
            {
                jm.code =1;
                jm.msg = result.ResultMsg;
            }
            return jm;
        }


        /// <summary>
        /// 信息添加
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> AddUserInfo(/*[FromBody]*/AddUserInfoViewModel userInfo)
        {
            var result = await _userInfoService.Create(userInfo);
            var jm = new WebApiCallBack();
            if (result)
            {
                jm.code = 0;
                jm.msg = "添加成功";
            }
            else
            {
                jm.code = 1;
                jm.msg = "网络打瞌睡了！";
            }
            return jm;
        }


        /// <summary>
        /// 数据更新
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<WebApiCallBack> UpdateUserInfo(UserInfo userInfo) /*更新接口*/
        {
            var result = await _userInfoService.Update(userInfo);
            var jm = new WebApiCallBack();
            if (result)
            {
                jm.code = 0;
                jm.msg = "更新成功";
            }
            else
            {
                jm.code = 1;
                jm.msg = "网络打瞌睡了！";
            }
            return jm;
        }

        /// <summary>
        /// 数据删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<WebApiCallBack> DataDelete(int? id)
        {
            var result = await _userInfoService.Delete(id);
            var jm = new WebApiCallBack();
            if (result)
            {
                jm.code = 0;
                jm.msg = "删除成功";
            }
            else
            {
                jm.code = 1;
                jm.msg = "网络打瞌睡了！";
            }
            return jm;
        }

        /// <summary>
        /// 获取数据记录详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<WebApiCallBack> QueryUserInfoDetail(int id)
        {
            var result = await _userInfoService.QueryUserInfoDetail(id);
            var jm = new WebApiCallBack();
            if (result != null)
            {
                jm.code = 0;
                jm.data = result;
            }
            else
            {
                jm.code = 1;
                jm.msg = "网络打瞌睡了！";
            }
            return jm;
        }
        /// <summary>
        /// 接口回调Json实体
        /// </summary>
        public class WebApiCallBack
        {
            /// <summary>
            /// 请求接口返回说明
            /// </summary>
            public string methodDescription { get; set; }


            /// <summary>
            /// 提交数据
            /// </summary>
            public object otherData { get; set; } = null;

            /// <summary>
            /// 状态码
            /// </summary>
            public bool status { get; set; } = false;

            /// <summary>
            /// 信息说明。
            /// </summary>
            public string msg { get; set; } = "接口响应成功";

            /// <summary>
            /// 返回数据
            /// </summary>
            public object data { get; set; }

            /// <summary>
            /// 返回编码
            /// </summary>
            public int code { get; set; } = 0;

            /// <summary>
            /// 返回行数
            /// </summary>
            public int count { get; set; } = 0;
        }
    }
}
