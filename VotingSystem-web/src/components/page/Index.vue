<template>
    <div class="container" style="width: 70%; margin: auto">
        <div class="header">
            <div class="logo">投票列表</div>
            <div class="header-right">
                <div class="header-user-con">
                    <!-- 用户头像 -->
                    <div class="user-avator">
                        <img src="../../assets/img/img.jpg" />
                    </div>
                    <!-- 用户名下拉菜单 -->
                    <el-dropdown class="user-name" trigger="click" @command="handleCommand">
                    <span class="el-dropdown-link">
                        {{username}}
                        <i class="el-icon-caret-bottom"></i>
                    </span>
                        <el-dropdown-menu slot="dropdown">
                            <el-dropdown-item divided command="loginout">退出登录</el-dropdown-item>
                        </el-dropdown-menu>
                    </el-dropdown>
                </div>
            </div>
        </div>
        <div>
            <el-input v-model="page.search" placeholder="姓名" class="handle-input mr10"></el-input>
            <el-button type="primary" icon="el-icon-search" @click="handleSearch">搜索</el-button>
        </div>
        <el-table
            :data="votes"
            stripe
            style="width: 100%">
            <el-table-column label="序号" width="55" align="center" min-width="1">
                <template slot-scope="scope">
                    {{scope.$index+1+(page.currentPage-1)*page.pageSize}}
                </template>
            </el-table-column>
            <el-table-column
                prop="title"
                label="标题"
                width="180">
            </el-table-column min-width="2">
                <el-table-column prop="type" label="类型" align="center" >
                <template slot-scope="scope">
                    <el-tag :type="!scope.row.type ? 'success':(scope.row ?'danger':'')"
                            >{{scope.row.type ? "多选":"单选"}}
                            </el-tag>
                </template>
            </el-table-column>
            <el-table-column prop="startTime" :formatter="dateFormat" label="开始时间" align="center"></el-table-column>
                <el-table-column prop="endTime" :formatter="dateFormat" label="结束时间" align="center"></el-table-column>
            <el-table-column label="操作" width="180" align="center" min-width="3">
                <template slot-scope="scope">
                    <el-button
                            type="text"
                            icon="el-icon-edit"
                            @click="handleEdit(scope.$index, scope.row)"
                            v-if="compare(scope.$index, scope.row) == 0"
                    >参与投票
                    </el-button>
                    <el-tag :type="compare(scope.$index, scope.row)==-1 ? 'warning':'danger'" v-else
                            >{{compare(scope.$index, scope.row)==-1 ? "未开始":"已结束"}}
                            </el-tag>
                </template>
            </el-table-column>
        </el-table>
        <div class="pagination">
            <el-pagination
                    background
                    layout="total, prev, pager, next, jumper"
                    current-page="page.currentPage"
                    page-size="page.pageSize"
                    :total="page.pageTotal"
                    @current-change="handlePageChange"
            ></el-pagination>
        </div>


        <!-- 查看弹出框 -->
        <el-dialog title="投票" :visible.sync="voteVis" width="30%">
            <template>
                <h2 style="margin-bottom: 20px">{{form.title}}</h2>

                <div v-if="!form.type">
                    <el-radio v-model="option" v-for="(item,index) in form.options" :key="item.id" :label="item.id" style="display:block;margin-top: 10px">
                        {{item.msg}}
                    </el-radio>
                </div>
                <el-checkbox-group v-model="options" v-else>
                    <el-checkbox class="choose" v-for="(item,index) in form.options" :key="item.id" :label="item.id">
                        {{item.msg}}
                    </el-checkbox>
                </el-checkbox-group>
            </template>
            <span slot="footer" class="dialog-footer">
                <el-button @click="voteVis = false">返 回</el-button>
                <el-button type="primary" @click="submit">投 票</el-button>
            </span>
        </el-dialog>
    </div>
</template>

<script>
import moment from 'moment'
    export default {
        name: 'index',
        data() {
            return {
                page:{
                    currentPage: 1,
                    pageSize: 10,
                    pageTotal: 0,
                    search: ''
                },
                votes: [],
                form: {},
                idx: -1,
                id: -1,
                voteVis: false,
                username: '',
                option: '',
                options:[]
            }
        },
        created() {
            this.getData();
            this.username = localStorage.getItem('ms_username');
        },
        methods: {
            dateFormat(row,column){
                var date = row[column.property];
                if(date === undefined){
                    return ''
                }
                return moment(date).format("YYYY-MM-DD HH:mm:ss")
            },
            compare(index, row){
                let ans;
                let date = new Date().getTime();
                let startTime = new Date(row.startTime).getTime();
                let endTime = new Date(row.endTime).getTime();
                if(date<=startTime) return -1;
                else if(date>=endTime) return 1;
                return 0;
            },
            getData() {
                //获取用户信息
                this.$axios.post('vote/findAll', this.page)
                    .then(response => {
                        var data = response.data;
                        this.page.pageTotal = data.totalCount;
                        this.votes = data.data;
                        console.log(data);
                    })
                    .catch(() => {
                        this.$message.error('用户列表加载失败');
                    });
                    
            },
            handleDelete(index, row) {
                console.log(Index, row);
            },
            // 编辑操作
            handleEdit(index, row) {
                this.$axios.post('poll/check', {
                    vid: Number(row.id)
                }).then(response => {
                    if(response.data.status == 200){
                        this.idx = index;
                        this.form = row;
                        this.voteVis = true;
                    }else{
                        this.$message.error("已投票，不能重复投票");
                    }
                }).catch(() => {
                    this.$message.error('未知错误，请重试');
                });
            },
            // 用户名下拉菜单选择事件
            handleCommand(command) {
                if (command == 'loginout') {
                    localStorage.removeItem('ms_username');
                    this.$router.push('/login');
                }
            },
            submit(){
                this.$axios.post('poll/add', {
                    vid: Number(this.form.id),
                    oids: this.form.type ? this.options : [this.option]
                }).then(response => {
                    if(response.data.status == 200){
                        this.$message.success('投票成功');
                        this.form={};
                        this.options=[];
                        this.voteVis = false;
                    }else{
                        this.$message.error('投票失败');
                    }
                }).catch(() => {
                    this.$message.error('投票失败');
                });
            }
        }
    };

</script>

<style scoped>
    .handle-box {
        margin-bottom: 20px;
    }

    .handle-input {
        width: 300px;
        display: inline-block;
    }

    .table {
        width: 100%;
        font-size: 14px;
    }

    .mr10 {
        margin-right: 10px;
    }

    .choose {
        display:block;
        margin-top: 20px;
    }
    .header {
        position: relative;
        box-sizing: border-box;
        width: 100%;
        height: 70px;
        font-size: 22px;
        color: #fff;
        margin-bottom: 20px;
    }

    .header .logo {
        float: left;
        width: 250px;
        line-height: 70px;
        margin-left: 20px;
    }
    .header-right {
        float: right;
        padding-right: 50px;
    }
    .header-user-con {
        display: flex;
        height: 70px;
        align-items: center;
    }

    .user-name {
        margin-left: 10px;
    }
    .user-avator {
        margin-left: 20px;
    }
    .user-avator img {
        display: block;
        width: 40px;
        height: 40px;
        border-radius: 50%;
    }
    .el-dropdown-link {
        color: #fff;
        cursor: pointer;
    }
</style>
