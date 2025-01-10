module.exports = async (migration) => {

migration.editContentType('columnLayout')
    .editField('columnCount').validations([
            {
                range: {
                min: 2,
                max: 4,
                },
        
                message: "Can only have a 2, 3, or 4 column layout",
            },
            {
                in: [2, 3, 4],
            },
        ]
    );
};
