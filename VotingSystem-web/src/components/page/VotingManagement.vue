<template>
    <div>
        <div class="crumbs">
            <el-breadcrumb separator="/">
                <el-breadcrumb-item>
                    <i class="el-icon-lx-calendar"></i> 投票管理
                </el-breadcrumb-item>
            </el-breadcrumb>
        </div>
        <div class="container">
            <div class="handle-box">
                <el-button
                        type="primary"
                        icon="el-icon-delete"
                        class="handle-del mr10"
                        @click="delAllSelection"
                >批量删除
                </el-button>
                <el-input v-model="page.search" placeholder="姓名" class="handle-input mr10"></el-input>
                <el-button type="primary" icon="el-icon-search" @click="handleSearch">搜索</el-button>
                <el-row style="float: right">
                    <el-button type="primary" @click="addVoteFlag=true">新增投票</el-button>
                </el-row>
            </div>
            <el-table
                    :data="votes"
                    border
                    class="table"
                    ref="multipleTable"
                    header-cell-class-name="table-header"
                    @selection-change="handleSelectionChange"
            >
                <el-table-column type="selection" width="55" align="center"></el-table-column>
                <el-table-column label="序号" width="55" align="center" min-width="1">
                    <template slot-scope="scope">
                        {{scope.$index+1+(page.currentPage-1)*page.pageSize}}
                    </template>
                </el-table-column>
                <el-table-column prop="title" label="标题" align="center" min-width="4"></el-table-column>
                <el-table-column prop="name" label="选项" min-width="4">
                    <template slot-scope="scope">
                        <p v-for="(item,index) in votes[scope.$index].options">{{item.choice}}、{{item.msg}}</p>
                    </template>
                </el-table-column>
                <el-table-column prop="type" label="类型" align="center" min-width="2">
                    <template slot-scope="scope">
                        
                        <el-tag :type="!scope.row.type ? 'success':(scope.row ?'danger':'')"
                                >{{scope.row.type ? "多选":"单选"}}
                                </el-tag>
                    </template>
                </el-table-column>
                <el-table-column prop="{startTime: startTime,endTime: endTime}" label="投票进度" align="center" min-width="5">
                    <template slot-scope="scope">
                        <el-progress :text-inside="true" :stroke-width="24" :percentage="getSchedule(scope.row)" ></el-progress>
                    </template>
                </el-table-column>
                <el-table-column prop="startTime" :formatter="dateFormat" label="开始时间" align="center" min-width="2"></el-table-column>
                <el-table-column prop="endTime" :formatter="dateFormat" label="结束时间" align="center" min-width="2"></el-table-column>
                <el-table-column prop="createTime" :formatter="dateFormat" label="创建时间" align="center" min-width="2"></el-table-column>
                <el-table-column label="操作" width="180" align="center" min-width="3">
                    <template slot-scope="scope">
                        <el-button
                                type="text"
                                icon="el-icon-zoom-in"
                                class="green"
                                @click="handleSelect(scope.$index, scope.row)"
                        >查看
                        </el-button>
                        <el-button
                                type="text"
                                icon="el-icon-edit"
                                @click="handleEdit(scope.$index, scope.row)"
                        >编辑
                        </el-button>
                        <el-button
                                type="text"
                                icon="el-icon-delete"
                                class="red"
                                @click="handleDelete(scope.$index, scope.row)"
                        >删除
                        </el-button>
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
        </div>

        <!-- 添加投票 -->
        <el-dialog title="新增投票" :visible.sync="addVoteFlag" width="30%">
            <el-form ref="form" :model="addVote" label-width="70px" style="width: 90%;">
                <el-form-item label="标题">
                    <el-input v-model="addVote.title"></el-input>
                </el-form-item>
                <el-form-item label="类型">
                    <el-select v-model="addVote.type" placeholder="请选择类型">
                        <el-option label="单选" value="0"></el-option>
                        <el-option label="多选" value="1"></el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="开始时间">
                    <el-date-picker
                            v-model="addVote.startTime"
                            type="datetime"
                            format="yyyy-MM-dd hh:mm"
                            value-format="yyyy-MM-dd hh:mm"
                            placeholder="选择日期时间">
                    </el-date-picker>
                </el-form-item>
                <el-form-item label="结束时间">
                    <el-date-picker
                            v-model="addVote.endTime"
                            type="datetime"
                            format="yyyy-MM-dd hh:mm"
                            value-format="yyyy-MM-dd hh:mm"
                            placeholder="选择日期时间">
                    </el-date-picker>
                </el-form-item>
                <el-form-item label="选项">
                    <div v-for="(item,index) in addVote.options">
                        <el-form-item :label="item.choice=(index+1)">
                            <el-input v-model="item.msg" style="width: 80%"></el-input>
                            <el-button type="danger" icon="el-icon-delete" circle style="margin-left: 2px" @click="deleteItems(addVote,item, index)"></el-button>
                        </el-form-item>
                    </div>
                </el-form-item>
                <el-form-item>
                    <el-button type="text" @click="addItem(addVote)" style="margin-left: 40%"><i class="el-icon-circle-plus"></i>新增选项</el-button>
                </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer">
                <el-button @click="addVoteFlag = false">取 消</el-button>
                <el-button type="primary" @click="addVoteSubmit">新 增</el-button>
            </span>
        </el-dialog>

        <!-- 编辑投票 -->
        <el-dialog title="修改投票信息" :visible.sync="editVoteFlag" width="30%">
            <el-form ref="form" :model="editVote" label-width="70px" style="width: 90%;">
                <el-form-item label="标题">
                    <el-input v-model="editVote.title"></el-input>
                </el-form-item>
                <el-form-item label="类型">
                    <el-select v-model="editVote.type" placeholder="请选择类型">
                        <el-option label="单选" value="0"></el-option>
                        <el-option label="多选" value="1"></el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="开始时间">
                    <el-date-picker
                            v-model="editVote.startTime"
                            type="datetime"
                            format="yyyy-MM-dd hh:mm"
                            value-format="yyyy-MM-dd hh:mm"
                            placeholder="选择日期时间">
                    </el-date-picker>
                </el-form-item>
                <el-form-item label="结束时间">
                    <el-date-picker
                            v-model="editVote.endTime"
                            type="datetime"
                            format="yyyy-MM-dd hh:mm"
                            value-format="yyyy-MM-dd hh:mm"
                            placeholder="选择日期时间">
                    </el-date-picker>
                </el-form-item>
                <el-form-item label="选项">
                    <div v-for="(item,index) in editVote.options">
                        <el-form-item :label="item.choice=(index+1)">
                            <el-input v-model="item.msg" style="width: 80%"></el-input>
                            <el-button type="danger" icon="el-icon-delete" circle style="margin-left: 2px" @click="deleteItems(editVote,item, index)"></el-button>
                        </el-form-item>
                    </div>
                </el-form-item>
                <el-form-item>
                    <el-button type="text" @click="addItem(editVote)" style="margin-left: 40%"><i class="el-icon-circle-plus"></i>新增选项</el-button>
                </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer">
                <el-button @click="editVoteFlag = false">取 消</el-button>
                <el-button type="primary" @click="updateVoteSubmit">修 改</el-button>
            </span>
        </el-dialog>

        <!-- 查看投票 -->
        <el-dialog title="投票详情" :visible.sync="selectVoteFlag" width="30%">
            <h3 style="margin-bottom: 10px;margin-left: 20px">{{voteDetail.title}}</h3>
            <el-table
            :data="voteDetail.options"
            style="width: 100%">
                <el-table-column prop="name" label="选项" width="180" >
                    <template slot-scope="scope">
                        {{scope.row.choice}}、{{scope.row.msg}}
                    </template>
                </el-table-column>
                <el-table-column prop="num" label="投票人数" align="center"></el-table-column>
                <el-table-column prop="num" label="占比" align="center" :formatter="percentage">
                </el-table-column>
            </el-table>
            <span slot="footer" class="dialog-footer">
                <el-button @click="selectVoteFlag = false">返 回</el-button>
            </span>
        </el-dialog>
    </div>
</template>

<script>
    import moment from 'moment'
    export default {
        name: 'basetable',
        data() {
            return {
                page:{
                    currentPage: 1,
                    pageSize: 10,
                    pageTotal: 0,
                    search: ''
                },
                votes: [],
                multipleSelection: [],
                delStr: '',
                //新增投票
                addVote: {
                    title: '',
                    type: '',
                    options: [
                        {choice:'',msg:''}
                    ],
                    startTime: '',
                    endTime: ''
                },
                addVoteFlag: false,
                editVoteFlag: false,
                selectVoteFlag: false,
                form: {},
                editVote: {},
                voteDetail: {},
                idx: -1,
                id: -1
            };
        },
        created() {
            this.getData();
        },
        methods: {
            dateFormat(row,column){
                var date = row[column.property];
                if(date === undefined){
                    return ''
                }
                return moment(date).format("YYYY-MM-DD HH:mm:ss")
            },
            //添加选项
            addItem(items){
                items.options.push({
                    option:'',
                    msg:''
                })
            },
            deleteItems(items,item, index) {
                this.index = items.options.indexOf(item);
                if (index !== 0) {
                    items.options.splice(index, 1)
                }
            },
            // 获取 easy-mock 的模拟数据
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
            //添加投票
            addVoteSubmit(){
                this.addVote.type = this.addVote.type=='1' ? true:false;
                this.addVote.startTime = new Date(this.addVote.startTime);
                this.addVote.endTime = new Date(this.addVote.endTime);
                //获取用户总数
                this.$axios.post('/vote/add',this.addVote)
                    .then(response => {
                        if(response.data.status == 200){
                            this.$message.success("新增成功");
                            this.addVoteFlag = false;
                        }else{
                            this.$message.error("新增失败");
                        }
                    });
            },
            //修改投票
            updateVoteSubmit(){
                this.editVote.type = this.editVote.type=='1' ? true:false;
                this.editVote.startTime = new Date(this.editVote.startTime);
                this.editVote.endTime = new Date(this.editVote.endTime);
                //获取用户总数
                this.$axios.post('/vote/update',this.editVote)
                    .then(response => {
                        if(response.data.status == 200){
                            this.$message.success("修改成功");
                            this.editVoteFlag = false;
                        }else{
                            this.$message.error("修改失败");
                        }
                    });
            },
            //翻页操作
            handlePageChange(currentPage){
                this.page.currentPage = currentPage;
                this.$axios.post('vote/findAll', this.page)
                    .then(response => {
                        var data = response.data;
                        this.page.pageTotal = data.totalCount;
                        this.votes = data.data;
                    })
                    .catch(() => {
                        this.$message.error('用户列表加载失败');
                    });
            },
            // 触发搜索按钮
            handleSearch() {
                this.handlePageChange(1);
            },
            // 删除操作
            handleDelete(index, row) {
                // 二次确认删除
                this.$confirm('确定要删除吗？', '提示', {
                    type: 'warning'
                })
                    .then(() => {
                        this.$axios
                            .post('/vote/delete',{id: row.id})
                            .then(response => {
                                if(response.data.status == 200){
                                    this.$message.success("删除成功");
                                }else{
                                    this.$message.error("删除失败");
                                }
                            })
                            .catch(()=>{
                                this.$message.error("删除失败");
                            });
                        this.votes.splice(index, 1);
                    })
                    .catch(() => {
                    });
            },
            // 多选操作
            handleSelectionChange(val) {
                this.multipleSelection = val;
            },
            delAllSelection() {
                this.delStr = '(';
                const length = this.multipleSelection.length;
                let str = '';
                for (let i = 0; i < length; i++) {
                    str += this.multipleSelection[i].name + ' ';
                    if(i != 0) this.delStr+=',';
                    this.delStr+=this.multipleSelection[i].id;
                }
                this.delStr+=')';
                this.$confirm('确定要删除吗？', '提示', {
                    type: 'warning'
                }).then(()=>{
                    this.$axios.post('/vote/deleteVotes',this.multipleSelection)
                        .then(response => {
                            if(response.data.status == 200){
                                this.$message.success(`删除 ${str} 成功`)
                            }
                        })
                        .catch(() => this.$message.error("删除失败"));
                    this.multipleSelection = [];
                    this.getData();
                })
            },
            // 查看操作
            handleSelect(index, row) {
                this.selectVoteFlag = true;
                this.$axios
                    .post('/vote/detail',row)
                    .then(response => {
                        var data = response.data;
                        if(data.status == 200){
                            this.voteDetail = data.data;
                            let len = this.voteDetail.options.lenght;
                        }else{
                            this.$message.error("访问失败，请重试");
                        }
                    })
                    .catch(()=>this.$message.error("访问失败，请重试"));
            },
            // 编辑操作
            handleEdit(index, row) {
                this.idx = index;
                this.editVote = row;
                this.editVote.type = row.type ? "多选" : "单选";
                this.editVoteFlag = true;
            },
            // 保存编辑
            update() {
                this.form.age = Number(this.form.age);
                this.$axios
                    .post('/user/update',this.form)
                    .then(response => {
                        var data = response.data;
                        if(data.status == 200){
                            this.$message.success("用户信息更新成功");
                        }else{
                            this.$message.error("用户信息更新失败");
                        }
                    })
                    .catch(()=>this.$message.error("用户信息更新失败"));
                this.editVisible = false;
                this.$set(this.votes, this.idx, this.form);
            },
            //获取投票进度百分比
            getSchedule(row){
                let ans;
                let date = new Date().getTime();
                let startTime = new Date(row.startTime).getTime();
                let endTime = new Date(row.endTime).getTime();
                if(date<=startTime) ans = 0;
                else if(date>=endTime) ans = 100;
                else ans = Number(date-startTime)*100/Number(endTime-startTime);
                return ans.toFixed(1);
            },
            percentage(row,column){
                let num = row[column.property];
                if(this.voteDetail.count==0) return "暂无人投票";
                return Number(num/this.voteDetail.count).toFixed(2)*100+"%";
            },
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

    .red {
        color: #ff0000;
    }

    .green {
        color: #296100;
    }

    .mr10 {
        margin-right: 10px;
    }

    .table-td-thumb {
        display: block;
        margin: auto;
        width: 40px;
        height: 40px;
    }
</style>
