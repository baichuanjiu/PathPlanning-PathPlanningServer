# 目录

- [目录](#目录)
- [写在前面](#写在前面)
- [效果预览](#效果预览)
  - [借助深度学习模型的道路信息提取](#借助深度学习模型的道路信息提取)
  - [使用OpenCV完成的道路网结构的提取](#使用opencv完成的道路网结构的提取)
- [运行](#运行)

# 写在前面

本仓库为PathPlanning项目后端仓库，主要实现功能为：
- 部署提取出的ONNX模型实现道路信息的提取
- 通过OpenCV完成道路网结构的提取
  
若要查看更多有关PathPlanning项目的信息，可[点击此链接](https://github.com/baichuanjiu/PathPlanning-DeepLearning)前往主体仓库。

# 效果预览

## 借助深度学习模型的道路信息提取

![道路识别（原图）.png](https://github.com/baichuanjiu/ReadMeImages/blob/main/PathPlanning/%E9%81%93%E8%B7%AF%E8%AF%86%E5%88%AB%EF%BC%88%E5%8E%9F%E5%9B%BE%EF%BC%89.png?raw=true)  
![道路识别（结果）.png](https://github.com/baichuanjiu/ReadMeImages/blob/main/PathPlanning/%E9%81%93%E8%B7%AF%E8%AF%86%E5%88%AB%EF%BC%88%E7%BB%93%E6%9E%9C%EF%BC%89.png?raw=true)

## 使用OpenCV完成的道路网结构的提取

算法步骤如下：
1. 进行道路的细化
2. 判断哪些点是道路的端点或交叉点
3. 第2步容易在相近位置判断出多个候选点，需要对相聚较近的点进行合并

效果如下：
![道路网结构提取（原图）.png](https://github.com/baichuanjiu/ReadMeImages/blob/main/PathPlanning/%E9%81%93%E8%B7%AF%E7%BD%91%E7%BB%93%E6%9E%84%E6%8F%90%E5%8F%96%EF%BC%88%E5%8E%9F%E5%9B%BE%EF%BC%89.png?raw=true) 
![道路网结构提取（细化）.png](https://github.com/baichuanjiu/ReadMeImages/blob/main/PathPlanning/%E9%81%93%E8%B7%AF%E7%BD%91%E7%BB%93%E6%9E%84%E6%8F%90%E5%8F%96%EF%BC%88%E7%BB%86%E5%8C%96%EF%BC%89.png?raw=true) 
![道路网结构提取（结果）.png](https://github.com/baichuanjiu/ReadMeImages/blob/main/PathPlanning/%E9%81%93%E8%B7%AF%E7%BD%91%E7%BB%93%E6%9E%84%E6%8F%90%E5%8F%96%EF%BC%88%E7%BB%93%E6%9E%9C%EF%BC%89.png?raw=true) 


# 运行

运行前需要先将训练好的模型提取成ONNX模型，并将其（dlinknet.onnx）放入PathPlanning/Tools/DLinkNetInference文件夹下。  
由于dlinknet.onnx文件体积过大（超过100M），无法上传至github，故请自行训练并提取或联系我以获取。  
关于如何训练与提取ONNX模型，请参阅[主体仓库]((https://github.com/baichuanjiu/PathPlanning-DeepLearning))。
